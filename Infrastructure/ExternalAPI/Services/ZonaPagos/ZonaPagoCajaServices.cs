using Aplication.Interfaces.ZonaPagos;
using Infraestructure.ExternalAPI.DTOs.ZonaPagos;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Aplication.Services.Recaudos.ZonaPagoCaja
{
    public sealed class ZonaPagoCajaServices(
        IOptions<ClavesMonterrey> options_monterrey,
        IPasarelaServices pasarela_services,
        IOptions<ClavesCAJAZP> options_zp,
        IIndustria industria,
        DataContext context
    ) : IZonaPagoCaja
    {
        private readonly IOptions<ClavesMonterrey> _optionsMonterrey = options_monterrey;
        private readonly IPasarelaServices _pasarelaServices = pasarela_services;
        private readonly IOptions<ClavesCAJAZP> _options_zp = options_zp;
        private readonly IIndustria _industria = industria;
        private readonly DataContext _context = context;

        public async Task<ConsultaPagoCajaDto> ConsultarFactura(ConsultaPagoCajaParams _)
        {
            var response = new ConsultaPagoCajaDto();
            try
            {
                int comercio_perteneciente = ObtenerComercioPerteneciente(_);

                if (comercio_perteneciente == 0)
                {
                    return ErrorResponseConsulta("2");                    
                }

                var existe_pago = await _context.RECAUDOSBANCOS.FirstOrDefaultAsync(r =>
                    r.codigo_recaudo == _.Referencia_pago
                    && r.codigo_estado_recaudo == EstadoRecaudo.Aprobado
                    && r.codigo_proveedor_recaudo
                        == (comercio_perteneciente == 1 ? ProveedorRecaudo.ZonaPagos : ProveedorRecaudo.BancoBogota)
                );

                if (existe_pago != null)
                    return ErrorResponseConsulta("1");

                bool credenciales_validas = ValidarCredenciales(_);

                if (!credenciales_validas)
                    return ErrorResponseConsulta("2");

                string ip_server_comercio = _pasarelaServices.ObtenerIPComercio(_.Id_Comercio.ToString());

                if (string.IsNullOrEmpty(ip_server_comercio))
                    return ErrorResponseConsulta("2");

                var referencia = _.Referencia_pago ?? string.Empty;
                var usuario = referencia.Length >= 10 ? referencia[..10] : referencia;
                var llave = referencia.Length >= 18 ? referencia[..18] : referencia;
                var recibo = referencia.Length >= 24 ? referencia[18..24] : (referencia.Length > 18 ? referencia[18..] : string.Empty);

                var consulta_recibo = await _industria.IYC003R1(
                    new RequestIYC003R1
                    {
                        sesion = "",
                        usuario = usuario,
                        llave = llave,
                        recibo = recibo,
                        validar_rbo = "S",
                    },
                    ip_server_comercio
                );

                if (consulta_recibo.STATUS != "00")
                    return ErrorResponseConsulta("2");

                var total_pagar_recibo = _pasarelaServices.CalcularTotalFactura(consulta_recibo);
                var info_adicional = GenerarInfoAdicional(consulta_recibo.MENSAJE);

                //Variables creadas para Monterrey - Banco Bogotá
                response.Referencia_factura = ip_server_comercio == _optionsMonterrey.Value.IpMonterrey ? _.Referencia_pago : null;
                response.Valor_referencia_factura =
                    ip_server_comercio == _optionsMonterrey.Value.IpMonterrey ? total_pagar_recibo.ToString() : null;
                //--------------------------------------------------------

                response.Fecha_limite_pago = DateTime.Now.ToString("dd/MM/yyyy");
                response.Descripcion_estado = GenerarEstadoConsulta("0");
                response.Valor_factura = _options_zp.Value.IpVillanueva == ip_server_comercio ? total_pagar_recibo : null;
                response.Info_Adicional = info_adicional;
                response.Codigo_Estado = "0";
            }
            catch (Exception)
            {
                response = ErrorResponseConsulta("1");
            }
            return response;
        }

        private static ConsultaPagoCajaDto ErrorResponseConsulta(string cod_status) =>
            new() { Descripcion_estado = GenerarEstadoConsulta(cod_status), Codigo_Estado = cod_status };

        public bool ValidarCredenciales(ConsultaPagoCajaParams datos)
        {
            string pwdComercio;
            int idComercio;
            int idBanco;

            if (_options_zp.Value.IntIdComercio.ToString() == datos.Id_Comercio.ToString())
            {
                pwdComercio = _options_zp.Value.StrPwdComercio;
                idComercio = _options_zp.Value.IntIdComercio;
                idBanco = _options_zp.Value.IntIdBanco;
            }
            else if (_optionsMonterrey.Value.IntIdComercio.ToString() == datos.Id_Comercio.ToString())
            {
                pwdComercio = _optionsMonterrey.Value.StrPwdComercio;
                idComercio = _optionsMonterrey.Value.IntIdComercio;
                idBanco = _optionsMonterrey.Value.IntIdBanco;
            }
            else
                return false;

            return datos.Id_Banco == idBanco
                && datos.Id_Comercio == idComercio
                && !string.IsNullOrEmpty(datos.Password)
                && datos.Password == pwdComercio
                && !string.IsNullOrEmpty(datos.Referencia_pago);
        }

        public int ObtenerComercioPerteneciente(ConsultaPagoCajaParams datos)
        {
            if (_options_zp.Value.IntIdComercio.ToString() == datos.Id_Comercio.ToString())
                return 1;
            else if (_optionsMonterrey.Value.IntIdComercio.ToString() == datos.Id_Comercio.ToString())
                return 2;
            else
                return 0;
        }

        public static string GenerarInfoAdicional(MensajeIYC003R1 mensaje)
        {
            var campos = new[]
            {
                mensaje.id,
                mensaje.ano,
                mensaje.periodo,
                mensaje.nomid,
                mensaje.apeid,
                mensaje.razon,
                mensaje.tipo_id,
                mensaje.direcc,
                mensaje.ciudad,
                mensaje.correo,
                mensaje.telefono,
                mensaje.imp_pago,
                mensaje.avi_pago,
                mensaje.bom_pago,
                mensaje.otro_pago,
                mensaje.int_ad,
                mensaje.total_impto,
                mensaje.sancion_min,
                mensaje.monto_int,
                mensaje.nro_dias,
                mensaje.nit_usunet,
                mensaje.fecha_limit,
                mensaje.ret_ica,
                mensaje.autoret_ica,
                mensaje.pago_inter,
                mensaje.fecha_vence,
            };

            return string.Join("|", campos.Select(c => c?.ToString()?.Trim() ?? " "));
        }

        public async Task<AsientoPagoCajaDto> AsentarPago(AsientoPagoCajaParams _)
        {
            var response = new AsientoPagoCajaDto();
            try
            {
                int comercio_perteneciente = ObtenerComercioPerteneciente(_);

                if (comercio_perteneciente == 0)
                    return ErrorResponseAsiento("2", "W");

                bool credenciales_validas = ValidarCredenciales(_);

                if (!credenciales_validas)
                    return ErrorResponseAsiento("2", "W");

                if (_ == null || string.IsNullOrEmpty(_.Referencia_pago))
                    return ErrorResponseAsiento("2", "W");

                var guardado_data = new RECAUDOSBANCOS
                {
                    codigo_recaudo = _.Referencia_pago,
                    total_recaudo = _.Valor_pagado,
                    json_notificacion_recaudo = JsonSerializer.Serialize(_),
                    codigo_comercio_recaudo =
                        comercio_perteneciente == 1 ? ComercioRecaudo.MunicipioVillanueva : ComercioRecaudo.MunicipioMonterrey,
                    codigo_proveedor_recaudo = comercio_perteneciente == 1 ? ProveedorRecaudo.ZonaPagos : ProveedorRecaudo.BancoBogota,
                    codigo_estado_recaudo = EstadoRecaudo.Aprobado,
                    id_transaccion = _.Id_transaccion,
                };

                string ip_server_comercio = _pasarelaServices.ObtenerIPComercio(_.Id_Comercio.ToString());

                if (string.IsNullOrEmpty(ip_server_comercio))
                    return ErrorResponseAsiento("2", "E");

                //Grabar estado en power
                var response_dll = await _pasarelaServices.ActualizarEstadoPowerICA(
                    new RequestIYC007
                    {
                        sesion = "",
                        usuario = guardado_data.codigo_recaudo[..10],
                        llave_imp = guardado_data.codigo_recaudo,
                        estado = "P", // Estado primera vez siempre P
                    },
                    ip_server_comercio
                );

                guardado_data.estado_recaudo_cobol = response_dll.STATUS;
                guardado_data.detalle_recaudo_cobol = response_dll.MENSAJE;

                // Validar si ya existe un recaudo con el mismo código y estado A
                bool existe_recaudo = await _context.RECAUDOSBANCOS.AnyAsync(r =>
                    r.codigo_recaudo == _.Referencia_pago
                    && r.codigo_estado_recaudo == EstadoRecaudo.Aprobado
                    && r.codigo_proveedor_recaudo
                        == (comercio_perteneciente == 1 ? ProveedorRecaudo.ZonaPagos : ProveedorRecaudo.BancoBogota)
                );

                if (existe_recaudo)
                {
                    return ErrorResponseAsiento("2", "E");
                }

                _context.RECAUDOSBANCOS.Add(guardado_data);
                var result = await _context.SaveChangesAsync();

                (string estado_asiento, string tipo_mensaje) = result switch
                {
                    0 => ("2", "E"),
                    1 => ("0", "I"),
                    _ => ("1", "W"),
                };

                response.Descripcion = GenerarEstadoAsiento(estado_asiento);
                response.Codigo_estado = estado_asiento;
                response.Severidad = tipo_mensaje;
            }
            catch (Exception)
            {
                return ErrorResponseAsiento("1", "E");
            }
            return response;
        }

        private static AsientoPagoCajaDto ErrorResponseAsiento(string cod_status, string severidad) =>
            new()
            {
                Descripcion = GenerarEstadoAsiento(cod_status),
                Codigo_estado = cod_status,
                Severidad = severidad,
            };

        public static string GenerarEstadoConsulta(string estado)
        {
            return estado switch
            {
                "0" => "Exitoso",
                "1" => "Factura no disponible para pago / Cliente no existe",
                _ => "Ocurrio un error inesperado en la operación",
            };
        }

        public static string GenerarEstadoAsiento(string estado)
        {
            return estado switch
            {
                "0" => "Se realizó exitosamente la actualización del pago",
                "1" => "No se pudo realizar la actualización del pago",
                _ => "Ocurrió un error inesperado en la operación",
            };
        }
    }
}
