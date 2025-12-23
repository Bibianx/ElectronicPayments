using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Aplication.DTOs.Inventarios;
using Aplication.Interfaces;
using Domain.Entities.Epayco;
using Infraestructure.ExternalAPI.DTOs.Epayco;
using Infrastructure.ExternalAPI.Common.Response;
using Microsoft.Extensions.Options;

namespace Infrastructure.ExternalAPI.Services.Epayco
{
    public class EpaycoServices(
        IHttpClientFactory httpClientFactory,
        IOptions<EpaycoCredentials> options,
        IPasarelaServices pasarelaServices,
        ILogger<EpaycoServices> logger,
        DataContext context,
        IMapper mapper
    ) : IEpayco
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IPasarelaServices _pasarelaServices = pasarelaServices;
        private readonly IOptions<EpaycoCredentials> _options = options;
        private readonly ILogger<EpaycoServices> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly DataContext _context = context;
        private readonly SemaphoreSlim _tokenLock = new(1, 1);
        private DateTimeOffset _tokenExpiresAt;
        private TokenResponseEpayco _cachedToken;

        private async Task<TokenResponseEpayco> ObtenerTokenCache()
        {
            await _tokenLock.WaitAsync();
            try
            {
                if (_cachedToken is not null && DateTimeOffset.UtcNow < _tokenExpiresAt)
                {
                    return _cachedToken;
                }

                var nuevo_token = await Login();

                if (string.IsNullOrEmpty(nuevo_token.data))
                {
                    throw new Exception("Error al generar token de autenticación Epayco");
                }

                _cachedToken = new TokenResponseEpayco { token = nuevo_token.data };
                _tokenExpiresAt = DateTimeOffset.UtcNow.AddSeconds(3600 - 30);
                return _cachedToken;
            }
            finally
            {
                _tokenLock.Release();
            }
        }

        public async Task<ServiceResponse<string>> Login()
        {
            var response = new ServiceResponse<string>();
            try
            {
                string PublicKey = _options.Value.PublicKey;
                string PrivateKey = _options.Value.PrivateKey;

                if (string.IsNullOrEmpty(PublicKey) || string.IsNullOrEmpty(PrivateKey))
                {
                    response.success = false;
                    response.message = "Las credenciales de Epayco no están configuradas correctamente.";
                    return response;
                }

                var auth_token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{PublicKey}:{PrivateKey}"));

                var epayco_client = _httpClientFactory.CreateClient("Epayco");
                var request = new HttpRequestMessage(HttpMethod.Post, "login")
                {
                    Content = new StringContent("", Encoding.UTF8, "application/json"),
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", auth_token);
                var result = await epayco_client.SendAsync(request);

                if (!result.IsSuccessStatusCode)
                {
                    response.success = false;
                    response.message = $"Error al generar el token, status code: {result.StatusCode}";
                    return response;
                }

                var content = await result.Content.ReadAsStringAsync();
                var token_response = JsonSerializer.Deserialize<TokenResponseEpayco>(content);

                response.data = token_response.token;
                response.success = true;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "Ocurrió un error al generar el token.";
                response.error = $"error: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<INV805FResponse>> ObtenerFacturasCliente(INV805FParams _)
        {
            var response = new ServiceResponse<INV805FResponse>();
            try
            {
                var response_dll = await _pasarelaServices.EjecutarDllGenerica<INV805FParams, INV805FResponse>(
                    _options.Value.IpComercializadora,
                    @"MAIN-ELECT/APP/INVENT/INV805F.DLL",
                    _
                );

                response.success = true;
                response.message = "Facturas obtenidas correctamente.";
                response.data = response_dll;
            }
            catch (Exception)
            {
                response.success = false;
                response.message = "Ocurrió un error al obtener las facturas del cliente.";
                response.data = null;
            }
            return response;
        }

        public async Task<ServiceResponse<TransactionResponse>> TransaccionPSE(TransactionRequest _)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            var response = new ServiceResponse<TransactionResponse>();
            try
            {
                var pago_pendiente = await _context.EPAYCO.FirstOrDefaultAsync(e =>
                    new[] { "P", "A" }.Contains(e.estado) && e.factura.Equals(_.invoice)
                );

                if (pago_pendiente != null)
                {
                    response.success = false;
                    response.message = $"La factura {_.invoice} ya fue registrada con estado {pago_pendiente.estado}";
                    return response;
                }

                var autorizacion = await ObtenerTokenCache();
                var epayco_client = _httpClientFactory.CreateClient("Epayco");
                epayco_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", autorizacion.token);

                var result = await epayco_client.PostAsJsonAsync("payment/process/pse", _);

                var root = await result.Content.ReadAsStringAsync();
                var data = await result.Content.ReadFromJsonAsync<TransactionResponse>();

                if (!data.success)
                {
                    response.success = false;
                    response.message = "Ocurrió un error al ejecutar la transacción.";
                    response.error = $"Error: {result.StatusCode}";
                    return response;
                }

                response.success = true;
                response.message = "Transacción ejecutada correctamente.";
                response.data = data;

                var epayco_data = _mapper.Map<EPAYCO>(data);
                _context.EPAYCO.Add(epayco_data);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.success = false;
                response.message = "Ocurrió un error al ejecutar la transacción.";
                response.error = $"Error {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<List<facturasFiltradasDto>>> FiltrarFacturasProcesadas()
        {
            var response = new ServiceResponse<List<facturasFiltradasDto>>();
            try
            {
                var db_facturas = await _context
                    .EPAYCO.Where(f => new[] { "P", "A" }.Contains(f.estado))
                    .AsNoTracking()
                    .Select(e => new facturasFiltradasDto
                    {
                        id_pago = e.id_pago,
                        factura = e.factura,
                        estado = e.estado,
                        valor = e.valor,
                    })
                    .ToListAsync();

                if (db_facturas.Count == 0)
                {
                    response.success = true;
                    response.data = [];
                    return response;
                }

                response.success = true;
                response.data = db_facturas;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "Ocurrió un error al cargar las facturas.";
                response.error = $"Error: {ex.Message}";
            }
            return response;
        }

        public async Task ProcesarWebHook(WebhookDto data)
        {
            try
            {
                string PKey = _options.Value.PKey;
                string PCustIdCliente = _options.Value.PCustIdCliente;

                if (string.IsNullOrEmpty(PKey) || string.IsNullOrEmpty(PCustIdCliente))
                {
                    _logger.LogError("Las credenciales de Epayco no están configuradas correctamente.");
                    return;
                }

                var crear_firma = GenerarFirma(
                    PCustIdCliente,
                    PKey,
                    data.x_ref_payco.ToString(),
                    data.x_transaction_id,
                    data.x_amount.ToString(),
                    data.x_currency_code
                );

                if (!string.Equals(crear_firma, data.x_signature, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning(
                        "Firma inválida. Calculada: {FirmaCalculada}, Recibida: {FirmaRecibida}",
                        crear_firma,
                        data.x_signature
                    );
                    return;
                }

                var buscar_transaccion = await _context.EPAYCO.FirstOrDefaultAsync(e =>
                    e.transactionID.Equals(data.x_approval_code) && e.estado.Equals("P")
                );

                if (buscar_transaccion == null)
                {
                    _logger.LogWarning("No se encontró transacción pendiente para TransactionID: {TransactionID}", data.x_transaction_id);
                    return;
                }

                buscar_transaccion.estado =
                    (data.x_cod_respuesta == 1 && string.Equals(data.x_respuesta, "Aceptada", StringComparison.OrdinalIgnoreCase))
                        ? "A"
                        : "R";

                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Transacción {Estado} para TransactionID {TransactionID}",
                    buscar_transaccion.estado == "A" ? "aprobada" : "rechazada",
                    data.x_transaction_id
                );

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la notificación de la pasarela de pago.");
            }
        }

         public async Task<ServiceResponse<TransactionConfirmResponse>> ConfirmacionPSE(TransactionConfirmRequest _)
        {
            var response = new ServiceResponse<TransactionConfirmResponse>();
            try
            {
                var autorizacion = await ObtenerTokenCache();

                var epayco_client = _httpClientFactory.CreateClient("Epayco");
                epayco_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", autorizacion.token);

                var result = await epayco_client.PostAsJsonAsync("payment/pse/transaction", _);

                if (!result.IsSuccessStatusCode)
                {
                    response.success = false;
                    response.message = "Ocurrió un error al confirmar la transacción.";
                    response.error = $"Error: {result.StatusCode}";
                    return response;
                }

                var data = await result.Content.ReadFromJsonAsync<TransactionConfirmResponse>();
                response.success = true;
                response.data = data;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "Ocurrió un error al confirmar la transacción.";
                response.error = $"Error {ex.Message}";
            }

            return response;
        }

        private static string GenerarFirma(
            string custId,
            string pKey,
            string refPayco,
            string transactionId,
            string amount,
            string currency
        )
        {
            var raw = $"{custId}^{pKey}^{refPayco}^{transactionId}^{amount}^{currency}";
            var bytes = System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(raw));
            return string.Concat(bytes.Select(b => b.ToString("x2")));
        }

    }
}
