using Infrastructure.ExternalAPI.Common.Response;
using Infraestructure.ExternalAPI.DTOs.ZonaPagos;
using Microsoft.Extensions.Options;
using Domain.Entities.ZonaPagos;

namespace Aplication.Services.ZonaPagos
{
    public interface IZonaPagoPSE
    {
        Task<VerificacionPagoPSEResponse> VerificarPago(VerificacionPagoPSEParams _);
        Task<InicioPagoResponsePSEDto> IniciarPago(InicioPagoPSEParams _);
        Task<ServiceResponse<List<FacturaParams>>> CargasFacturas();
        Task ProcesarWebHook(int id_comercio, string id_pago);
    }

    public class ZonaPagoPSEServices(
        IHttpClientFactory httpClientFactory,
        ILogger<ZonaPagoPSEServices> logger,
        IPasarelaServices pasarelaServices,
        IOptions<ClavesPSE> options,
        DataContext context,
        IMapper mapper
    ) : IZonaPagoPSE
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IPasarelaServices _pasarelaServices = pasarelaServices;
        private readonly ILogger<ZonaPagoPSEServices> _logger = logger;
        private static readonly string[] list_estados = ["P", "A"];
        private readonly IOptions<ClavesPSE> _claves = options;
        private readonly DataContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<InicioPagoResponsePSEDto> IniciarPago(InicioPagoPSEParams pago)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            var response = new InicioPagoResponsePSEDto();

            try
            {
                pago.InformacionSeguridad.int_id_comercio = _claves.Value.IntIdComercio;
                pago.InformacionSeguridad.str_usuario = _claves.Value.StrUsrComercio;
                pago.InformacionSeguridad.str_clave = _claves.Value.StrPwdComercio;
                pago.InformacionSeguridad.int_modalidad = -1;

                if (!ValidarDatos(pago))
                    return ErrorInicioPagoResponse(-1);

                // Verificar si ya existe un intento pendiente o aprobado, si es así, no permitir nuevo intento con la misma factura
                var existe_intento = await _context
                    .INTENTOSZP.Where(i => i.str_id_pago == pago.InformacionPago.str_id_pago)
                    .OrderByDescending(i => i.fecha_intento)
                    .ThenByDescending(i => i.hora_intento)
                    .FirstOrDefaultAsync();

                if (existe_intento != null && list_estados.Contains(existe_intento.estado_intento))
                {
                    response.int_codigo = 2;
                    response.str_cod_error = "-1";
                    response.str_descripcion_error =
                        existe_intento.estado_intento == "P"
                            ? "Ya existe un intento pendiente para esta factura. Debe esperar a que finalice el proceso."
                            : "La factura ya fue pagada y aprobada. No puede generar un nuevo intento.";

                    return response;
                }

                var client = _httpClientFactory.CreateClient("ZonaPagos");

                var httpResult = await client.PostAsJsonAsync("Apis_CicloPago/api/InicioPago", pago);
                if (!httpResult.IsSuccessStatusCode)
                    return ErrorInicioPagoResponse(-1);

                var data = await httpResult.Content.ReadFromJsonAsync<InicioPagoResponsePSEDto>();
                if (data == null || data.int_codigo != 1)
                    return ErrorInicioPagoResponse(-1);

                string id_comercio = pago.InformacionSeguridad.int_id_comercio.ToString();
                string ip_server_comercio = _pasarelaServices.ObtenerIPComercio(id_comercio);

                if (string.IsNullOrEmpty(ip_server_comercio))
                    return ErrorInicioPagoResponse(-1);

                //Grabar estado en power
                var response_dll = await _pasarelaServices.ActualizarEstadoPowerICA(
                    new RequestIYC007
                    {
                        sesion = "",
                        usuario = pago.InformacionPago.str_id_cliente,
                        llave_imp = pago.InformacionPago.str_id_pago,
                        estado = "P", // Estado primera vez siempre P
                    },
                    ip_server_comercio
                );

                string estado_cobol = response_dll.STATUS;
                string detalle_cobol = response_dll.MENSAJE;

                // Grabar intento e historial de intento en DB
                CrearIntentoEHistorial(pago, estado_cobol, detalle_cobol);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                response.str_descripcion_error = data?.str_descripcion_error;
                response.str_cod_error = data?.str_cod_error;
                response.str_url = data?.str_url;
                response.int_codigo = 1;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                response = ErrorInicioPagoResponse(-1);
            }
            return response;
        }

        private static InicioPagoResponsePSEDto ErrorInicioPagoResponse(int cod_status) =>
            new()
            {
                str_descripcion_error = GenerarEstadoIntento(cod_status),
                str_url = string.Empty,
                str_cod_error = "-1",
                int_codigo = 2,
            };

        private static VerificacionPagoPSEResponse ErrorVerificarPagoResponse(int cod_status) =>
            new()
            {
                str_detalle = GenerarEstadoIntento(cod_status),
                str_res_pago = string.Empty,
                int_cantidad_pagos = 0,
                int_estado = 2,
                int_error = cod_status,
            };

        private bool ValidarDatos(InicioPagoPSEParams pago)
        {
            if (
                string.IsNullOrEmpty(pago.InformacionPago.str_id_cliente)
                || string.IsNullOrEmpty(pago.InformacionPago.str_id_pago)
                || string.IsNullOrEmpty(pago.InformacionPago.str_tipo_id)
                || string.IsNullOrEmpty(pago.InformacionPago.str_email)
                || pago.InformacionPago.flt_total_con_iva <= 0
                || pago.InformacionPago.flt_valor_iva < 0
            )
            {
                return false;
            }

            if (
                pago.InformacionSeguridad.str_usuario != _claves.Value.StrUsrComercio
                || pago.InformacionSeguridad.str_clave != _claves.Value.StrPwdComercio
                || pago.InformacionSeguridad.int_id_comercio != _claves.Value.IntIdComercio
            )
            {
                return false;
            }

            return true;
        }

        private void CrearIntentoEHistorial(InicioPagoPSEParams pago, string estado_cobol, string detalle_cobol)
        {
            var intento = _mapper.Map<INTENTOSZP>(pago);
            _context.INTENTOSZP.Add(intento);
            _context.HISTORIALZP.Add(CrearHistorial(intento, estado_cobol, detalle_cobol));
        }

        public async Task<VerificacionPagoPSEResponse> VerificarPago(VerificacionPagoPSEParams datos)
        {
            try
            {
                if (
                    datos.str_usr_comercio != _claves.Value.StrUsrComercio
                    || datos.str_pwd_Comercio != _claves.Value.StrPwdComercio
                    || datos.int_id_comercio.ToString() != _claves.Value.IntIdComercio.ToString()
                )
                {
                    return ErrorVerificarPagoResponse(-1);
                }

                var client = _httpClientFactory.CreateClient("ZonaPagos");
                var result = await client.PostAsJsonAsync("Apis_CicloPago/api/VerificacionPago", datos);

                if (!result.IsSuccessStatusCode)
                {
                    return ErrorVerificarPagoResponse(-1);
                }

                var data = await result.Content.ReadFromJsonAsync<VerificacionPagoPSEResponse>();

                var ultimoIntento = BuscarUltimoIntento(data?.str_res_pago);
                data.str_res_pago = !string.IsNullOrEmpty(ultimoIntento) ? ultimoIntento + " ; " : string.Empty;
                data.int_cantidad_pagos = string.IsNullOrEmpty(ultimoIntento) ? 0 : 1;

                return data;
            }
            catch (Exception)
            {
                return ErrorVerificarPagoResponse(-1);
            }
        }

        public static string BuscarUltimoIntento(string str_res_pago)
        {
            if (string.IsNullOrWhiteSpace(str_res_pago))
                return string.Empty;

            var registros = str_res_pago
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(r => r.Trim())
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .ToList();

            if (registros.Count == 0)
                return string.Empty;

            string registroConMayorSegundoValor = null;
            decimal mayorValor = decimal.MinValue;
            bool existeEstado888 = false;

            foreach (var registro in registros)
            {
                var campos = registro.Split('|', StringSplitOptions.None).Select(c => c.Trim()).ToArray();

                if (campos.Length < 4)
                    continue;

                var estado_intento = campos[4];
                var consecutivo_intento = campos[1];

                if (estado_intento == "888")
                {
                    existeEstado888 = true;

                    if (decimal.TryParse(consecutivo_intento, out var valorActual))
                    {
                        if (valorActual > mayorValor)
                        {
                            mayorValor = valorActual;
                            registroConMayorSegundoValor = registro;
                        }
                    }
                }
            }

            if (existeEstado888 && registroConMayorSegundoValor != null)
                return registroConMayorSegundoValor;

            return registros.Last();
        }

        private static string GenerarEstadoIntento(int int_codigo)
        {
            return int_codigo switch
            {
                0 => "Se encontraron pagos",
                -1 => "Error en las validaciones realizadas",
                _ => "Error no identificado",
            };
        }

        private static HISTORIALZP CrearHistorial(INTENTOSZP intento, string estado_cobol, string detalle_cobol)
        {
            return new HISTORIALZP
            {
                fecha_ini = DateOnly.FromDateTime(DateTime.Now),
                hora_ini = TimeOnly.FromDateTime(DateTime.Now),
                detalle_pago = intento.str_descripcion_pago,
                descrip_estado_ini = "PENDIENTE",
                descrip_estado_fin = "PENDIENTE",
                detalle_cobol_ini = detalle_cobol,
                estado_cobol_ini = estado_cobol,
                id_pago = intento.id_pago,
                cod_estado_ini = "888",
                cod_estado_fin = "888",
                origen_cambio = "",
                estado_cobol_fin = "",
                detalle_cobol_fin = "",
            };
        }

        public async Task<ServiceResponse<List<FacturaParams>>> CargasFacturas()
        {
            var response = new ServiceResponse<List<FacturaParams>>();
            try
            {
                var db_estados = await _context
                    .INTENTOSZP.AsNoTracking()
                    .Select(i => new FacturaParams
                    {
                        id_pago = i.id_pago,
                        flt_total_con_iva = i.flt_total_con_iva,
                        flt_valor_iva = i.flt_valor_iva,
                        str_id_pago = i.str_id_pago,
                        str_descripcion_pago = i.str_descripcion_pago,
                        str_id_cliente = i.str_id_cliente,
                        estado_intento = i.estado_intento,
                        fecha_intento = i.fecha_intento,
                        hora_intento = i.hora_intento,
                    })
                    .Where(e => list_estados.Contains(e.estado_intento))
                    .OrderByDescending(e => e.fecha_intento)
                    .ThenByDescending(e => e.hora_intento)
                    .ToListAsync();

                if (db_estados.Count == 0)
                {
                    response.success = false;
                    response.message = "No hay facturas pendientes por aprobar";
                    return response;
                }
                response.data = db_estados;
                response.success = true;
            }
            catch (Exception)
            {
                response.success = false;
                response.message = "Ocurrió un error al consultar las facturas";
                response.error = "Error en la consulta de facturas";
            }
            return response;
        }

        public async Task ProcesarWebHook(int id_comercio, string id_pago)
        {
            if (id_comercio <= 0 || string.IsNullOrEmpty(id_pago))
                return;

            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var request = new VerificacionPagoPSEParams
                {
                    int_id_comercio = id_comercio,
                    str_usr_comercio = _claves.Value.StrUsrComercio,
                    str_pwd_Comercio = _claves.Value.StrPwdComercio,
                    str_id_pago = id_pago,
                    int_no_pago = -1,
                };

                var pago_verificado = await VerificarPago(request);

                if (pago_verificado.int_estado != 1 && pago_verificado.int_error != 0)
                {
                    _logger.LogError("Error al verificar el pago: {message}", pago_verificado);
                    return;
                }

                var consultar_historial = await _context
                    .HISTORIALZP.Include(h => h.intentos_zp)
                    .OrderByDescending(h => h.fecha_ini)
                    .ThenByDescending(h => h.hora_ini)
                    .FirstOrDefaultAsync(h => h.intentos_zp.str_id_pago.Equals(id_pago));

                if (consultar_historial == null)
                {
                    _logger.LogWarning("No se encontró historial para el pago {IdPago}", id_pago);
                    return;
                }

                consultar_historial.origen_cambio = "RUTA GET";
                await _pasarelaServices.ActualizarEstadoPago(consultar_historial, pago_verificado);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError("Error al procesar la notificación de pago: {message}", ex.Message);
            }
        }
    }
}
