using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Aplication.DTOs.Industria;
using Aplication.DTOs.ZonaPagos;
using Domain.Entities.ZonaPagos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using Models.Response;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Common
{
    public interface IPasarelaServices
    {
        Task<TResponse> EjecutarDllGenerica<TRequest, TResponse>(string direccion_ip_comercio, string directorio_dll, TRequest request)
            where TResponse : new();
        Task<EstructuraResponseDLL> ContabilizarFacturaAprobadaICA(RequestIYC006G request, string ip_server_comercio);
        Task<EstructuraResponseDLL> CrearTicketPagoAprobadoICA(RequestIYC005 request, string ip_server_comercio);
        Task<EstructuraResponseDLL> ActualizarEstadoPowerICA(RequestIYC007 request, string ip_server_comercio);
        (string int_pago_terminado, string int_estado_pago) ObtenerPropiedadesPipeline(string response);
        Task<ServiceResponse<object>> GetDll(string ip, string directorio, object datos_dll);
        Task ActualizarEstadoPago(HISTORIALZP pago, VerificarPagoResponseDto responseData);
        decimal CalcularTotalFactura(ResponseIYC003R1 response_factura);
        string GenerarEstadoPago(string int_estado_pago);
        string ObtenerIPComercio(string id_comercio);
    }

    public partial class PasarelaServices(
        IOptions<ClavesMonterrey> options_monterrey,
        IOptions<ClavesCAJAZP> options_caja,
        IOptions<ClavesPSE> options_pse,
        IConfiguration configuration,
        DataContext context
    ) : IPasarelaServices
    {
        private readonly IOptions<ClavesMonterrey> _optionsMonterrey = options_monterrey;
        private static readonly string[] array_estados = ["1", "1000", "888", "999"];
        private readonly IOptions<ClavesCAJAZP> options_caja = options_caja;
        private readonly IOptions<ClavesPSE> _options_pse = options_pse;
        private readonly IConfiguration _configuration = configuration;
        private readonly DataContext _context = context;

        public (string, string) ObtenerPropiedadesPipeline(string response)
        {
            var partes = (response ?? "").Split('|', StringSplitOptions.TrimEntries);
            return (partes.Length > 3 ? partes[3] : "", partes.Length > 4 ? partes[4] : "");
        }

        public string GenerarEstadoPago(string int_estado_pago)
        {
            if (string.IsNullOrEmpty(int_estado_pago))
                return "ERROR";

            return int_estado_pago switch
            {
                "200" => "PAGO INICIADO",
                "888" => "PAGO PENDIENTE POR INICIAR",
                "999" => "PAGO PENDIENTE POR FINALIZAR",
                "4001" => "PENDIENTE POR CR",
                "4000" => "RECHAZADO CR",
                "4003" => "ERROR CR",
                "1000" => "PAGO RECHAZADO",
                "1001" => "ERROR ENTRE ACH Y EL BANCO (RECHAZADA)",
                "1" => "PAGO FINALIZADO OK",
                _ => "ERROR",
            };
        }

        public string ObtenerIPComercio(string id_comercio)
        {
            if (id_comercio == options_caja.Value.IntIdComercio.ToString() || id_comercio == _options_pse.Value.IntIdComercio.ToString())
            {
                return options_caja.Value.IpVillanueva;
            }
            else if (id_comercio == _optionsMonterrey.Value.IntIdComercio.ToString())
            {
                return _optionsMonterrey.Value.IpMonterrey;
            }
            else
                return string.Empty;
        }

        //FUNCIONES PARA INDUSTRIA Y COMERCIO POWER 👌🫓
        public async Task<EstructuraResponseDLL> ActualizarEstadoPowerICA(RequestIYC007 request, string ip_server_comercio)
        {
            return await EjecutarDllGenerica<RequestIYC007, EstructuraResponseDLL>(
                ip_server_comercio,
                @"INDUSTRIA_V2/v2/app/impuesto/IYC007.DLL",
                request
            );
        }

        public async Task<EstructuraResponseDLL> ContabilizarFacturaAprobadaICA(RequestIYC006G request, string ip_server_comercio)
        {
            return await EjecutarDllGenerica<RequestIYC006G, EstructuraResponseDLL>(
                ip_server_comercio,
                @"INDUSTRIA_V2/v2/app/impuesto/IYC006G.DLL",
                request
            );
        }

        public async Task<EstructuraResponseDLL> CrearTicketPagoAprobadoICA(RequestIYC005 request, string ip_server_comercio)
        {
            return await EjecutarDllGenerica<RequestIYC005, EstructuraResponseDLL>(
                ip_server_comercio,
                @"INDUSTRIA_V2/v2/app/impuesto/IYC005.DLL",
                request
            );
        }

        //FUNCIONES PARA PREDIAL POWER 👌🫓
        // public async Task ContabilizarFacturaAprobadaPREDIAL(object request, string ip_server_comercio) //TRequest Y TResponse predial
        // {
        //     return await EjecutarDllGenerica<RequestIYC005, EstructuraResponseDLL>(
        //         ip_server_comercio,
        //         @"INDUSTRIA_V2/v2/app/impuesto/IYC005.DLL",
        //         request
        //     );
        // }

        //FUNCION GENERICA PARA EJECUTAR DLLS Y DESERIALIZAR DATA (tipado request y response) 👌🫓
        public async Task<TResponse> EjecutarDllGenerica<TRequest, TResponse>(
            string direccion_ip_comercio,
            string directorio_dll,
            TRequest request = default
        )
            where TResponse : new()
        {
            var response = new TResponse();
            try
            {
                var response_dll = await GetDll(direccion_ip_comercio, directorio_dll, request);

                var root = JsonSerializer.Deserialize<TResponse>(JsonSerializer.Serialize(response_dll.data));

                if (root != null)
                    return root;

                return response;
            }
            catch (Exception ex)
            {
                var statusProp = typeof(TResponse).GetProperty("STATUS");
                var mensajeProp = typeof(TResponse).GetProperty("MENSAJE");
                var programProp = typeof(TResponse).GetProperty("PROGRAM");

                statusProp?.SetValue(response, "99");
                mensajeProp?.SetValue(response, $"Error ejecutando DLL: {ex.Message}");
                programProp?.SetValue(response, "Error");

                return response;
            }
        }

        public decimal CalcularTotalFactura(ResponseIYC003R1 response_factura)
        {
            decimal total_impto = decimal.TryParse(response_factura.MENSAJE.total_impto?.ToString(), out decimal t1) ? t1 : 0;
            decimal sancion_min = decimal.TryParse(response_factura.MENSAJE.sancion_min?.ToString(), out decimal t2) ? t2 : 0;
            decimal int_ad = decimal.TryParse(response_factura.MENSAJE.int_ad?.ToString(), out decimal t3) ? t3 : 0;
            decimal monto_int = decimal.TryParse(response_factura.MENSAJE.monto_int?.ToString(), out decimal t4) ? t4 : 0;
            decimal nro_dias = decimal.TryParse(response_factura.MENSAJE.nro_dias?.ToString(), out decimal t5) ? t5 : 0;

            if (monto_int > 10) // dividir entre 100 cuando llega la cifra entera, para calculo correcto decimales
            {
                monto_int /= 100;
            }

            decimal subtotal = total_impto + sancion_min;
            decimal calculo_interes = subtotal * monto_int * nro_dias / 3000m;
            decimal interes = Math.Ceiling(calculo_interes / 1000m) * 1000m;
            decimal total = subtotal + interes + int_ad;
            return total;
        }

        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No se pudo encontrar la dirección IP local.");
        }

        public async Task<ServiceResponse<object>> GetDll(string ip, string directorio, object datos_dll)
        {
            ServiceResponse<object> response = new();
            try
            {
                var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromMinutes(20);
                var data = datos_dll;

                if (data != null)
                {
                    var jsonElement = JsonDocument.Parse(System.Text.Json.JsonSerializer.Serialize(data)).RootElement;
                    var formDataCollection = new List<KeyValuePair<string, string>>();

                    foreach (var property in jsonElement.EnumerateObject())
                    {
                        formDataCollection.Add(new KeyValuePair<string, string>(property.Name, property.Value.ToString()));
                    }

                    using HttpContent formContent = new FormUrlEncodedContent(formDataCollection);

                    ip ??= GetLocalIPAddress();

                    var request_uri = $"http://{ip}/{directorio}";

                    var startTime = DateTime.Now;
                    var response_message = await httpClient.PostAsync(request_uri, formContent).ConfigureAwait(false);
                    var endTime = DateTime.Now;
                    var timeElapsed = endTime - startTime;

                    if (response_message.IsSuccessStatusCode)
                    {
                        Regex regex = MyRegex();
                        var response_content = await response_message.Content.ReadAsStringAsync();

                        string resultado = regex.Replace(response_content, "");
                        string resultado_limpio = resultado.Replace("\n", "\\n");
                        System.Console.WriteLine($"conteido a parsear: {resultado}");
                        try
                        {
                            dynamic json_response = JsonDocument.Parse(resultado_limpio);
                            response.success = true;
                            response.message = "Dll consultado correctamente";
                            response.data = json_response;
                            return response;
                        }
                        catch (Exception ex)
                        {
                            response.success = false;
                            response.message = "Error al parsear el contenido del dll";
                            response.error = resultado_limpio;
                            response.data = $"Error al parsear {ex}";
                            return response;
                        }
                    }
                    else
                    {
                        response.success = false;
                        response.message = $"Error al realizar la solicitud al dll {response_message}, directorio {directorio} , ip. {ip}";
                        response.error = $"Error al realizar la solicitud al dll -> {response_message}";
                    }
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = $"Error en obtener informacion del dll  {ex.Message}";
                response.error = $"Error en obtener informacion del dll {ex}";
            }
            return response;
        }

        [GeneratedRegex("\\s{2,}")]
        private static partial Regex MyRegex();


        //FUNCION PARA ACTUALIZACION DE ESTADO PARTE WEB (DB) y POWER (CONTABILIDADES)👌🫓
        public async Task ActualizarEstadoPago(HISTORIALZP pago, VerificarPagoResponseDto responseData)
        {
            var (int_pago_terminado, int_estado_pago) = ObtenerPropiedadesPipeline(responseData.str_res_pago);

            var estados_pendientes = new[] { "888", "999", "4001" };
            pago.descrip_estado_fin = estados_pendientes.Contains(int_estado_pago) ? "PENDIENTE" : GenerarEstadoPago(int_estado_pago);

            if (!estados_pendientes.Contains(int_estado_pago))
            {
                pago.fecha_fin = DateOnly.FromDateTime(DateTime.Now);
                pago.hora_fin = TimeOnly.FromDateTime(DateTime.Now);
            }

            pago.cod_estado_fin = int_estado_pago;
            pago.json_respuesta = JsonSerializer.Serialize(responseData);

            if (array_estados.Contains(int_estado_pago))
            {
                string estado_final = int_estado_pago switch
                {
                    "1" => "A",
                    "1000" => "R",
                    "888" or "999" => "P",
                    _ => "E",
                };

                var intento = await _context
                    .INTENTOSZP.Where(i => i.str_id_pago.Equals(pago.intentos_zp.str_id_pago))
                    .OrderByDescending(i => i.fecha_intento)
                    .ThenByDescending(i => i.hora_intento)
                    .FirstOrDefaultAsync();

                if (intento != null)
                {
                    intento.estado_intento = estado_final;

                    var response_estado = await ActualizarEstadoPowerICA(
                        new RequestIYC007
                        {
                            sesion = "",
                            usuario = pago.intentos_zp.str_usuario,
                            llave_imp = intento.str_id_pago,
                            estado = estado_final,
                        },
                        ObtenerIPComercio(pago.intentos_zp.int_id_comercio.ToString())
                    );

                    pago.estado_cobol_fin = response_estado.STATUS;
                    pago.detalle_cobol_fin = response_estado.MENSAJE;

                    if (estado_final == "A")
                    {
                        if (pago.intentos_zp.str_opcional1 == "ICA")
                        {
                            var response_contab_ica = await ContabilizarFacturaAprobadaICA(
                                new RequestIYC006G
                                {
                                    sesion = "",
                                    usuario = pago.intentos_zp.str_usuario,
                                    llave_imp = pago.intentos_zp.str_id_pago,
                                    recibo = pago.intentos_zp.str_opcional2,
                                },
                                ObtenerIPComercio(pago.intentos_zp.int_id_comercio.ToString())
                            );
                            //Check contabilizacion
                            pago.contabilizado_cobol_ok = response_contab_ica?.STATUS == "00";

                            string ticket = pago.intentos_zp.numero_ticket_cobol.ToString().PadLeft(10, '0');
                            var response_ticket = await CrearTicketPagoAprobadoICA(
                                new RequestIYC005
                                {
                                    sesion = "",
                                    usuario = pago.intentos_zp.str_usuario,
                                    recibo = pago.intentos_zp.str_opcional2,
                                    ticket = ticket,
                                    paso = "2",
                                },
                                ObtenerIPComercio(pago.intentos_zp.int_id_comercio.ToString())
                            );
                            // //Check ticket pago
                            pago.ticket_cobol_ok = response_ticket?.STATUS == "00";
                        }
                        else
                        {
                            //TODO: Preguntar rutas dlls de PREDIAL
                            // await ContabilizarFacturaAprobadaPREDIAL(
                            //     new
                            //     {
                            //         usuario = "",
                            //         sesion = intento.str_usuario,
                            //         nro_cat = intento.str_id_pago,
                            //         ano_fin = DateTime.Now.Year.ToString(),
                            //         vlr_fac = intento.flt_total_con_iva,
                            //     },
                            //     ObtenerIPComercio(pago.intentos_zp.int_id_comercio.ToString())
                            // );
                        }
                    }
                }
            }
        }
    }
}
