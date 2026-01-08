using System.Text.Json;
using Aplication.DTOs.Predial;
using Aplication.Interfaces.ZonaPagos;
using Aplication.Services.Predial;
using Infraestructure.ExternalAPI.DTOs.ZonaPagos;
using Microsoft.Extensions.Options;

namespace Aplication.Services.Recaudos.ZonaPagoCaja
{
    public sealed class ZonaPagoCajaServices(
        IOptions<ClavesMonterrey> options_monterrey,
        IPasarelaServices pasarela_services,
        IOptions<ClavesCAJAZP> options_zp,
        IIndustria industria,
        IPredial predial,
        DataContext context
    ) : IZonaPagoCaja
    {
        private readonly IOptions<ClavesMonterrey> _optionsMonterrey = options_monterrey;
        private readonly IPasarelaServices _pasarelaServices = pasarela_services;
        private readonly IOptions<ClavesCAJAZP> _options_zp = options_zp;
        private readonly IIndustria _industria = industria;
        private readonly DataContext _context = context;
        private readonly IPredial _predial = predial;

        public static class CacheFactura
        {
            public static Dictionary<string, string> tipo_factura = [];
        }

        public async Task<ConsultaPagoCajaDto> ConsultarFactura(ConsultaPagoCajaParams _)
        {
            try
            {
                int comercio_perteneciente = ObtenerComercioPerteneciente(_);

                if (comercio_perteneciente == 0)
                    return ErrorResponseConsulta("2");

                var existe_pago = await _context.RECAUDOSBANCOS.FirstOrDefaultAsync(r =>
                    r.codigo_recaudo == _.Referencia_pago
                    && r.codigo_estado_recaudo == EstadoRecaudo.Aprobado
                    && r.codigo_proveedor_recaudo
                        == (comercio_perteneciente == 1 ? ProveedorRecaudo.ZonaPagos : ProveedorRecaudo.BancoBogota)
                );

                if (existe_pago != null)
                    return ErrorResponseConsulta("1");

                if (!ValidarCredenciales(_))
                    return ErrorResponseConsulta("2");

                string ip = _pasarelaServices.ObtenerIPComercio(_.Id_Comercio.ToString());

                if (string.IsNullOrEmpty(ip))
                    return ErrorResponseConsulta("2");

                var referencia = _.Referencia_pago ?? string.Empty;

                decimal total;
                string info;

                var (existe_ica, Total, Info) = await ConsultarIndustria(referencia, ip);

                if (existe_ica)
                {
                    total = Total;
                    info = Info;
                    CacheFactura.tipo_factura[referencia[9..24]] = "ICA";
                }
                else
                {
                    var existe_predial = await ConsultarPredial(referencia, ip);

                    if (!existe_predial.existe_ica)
                        return ErrorResponseConsulta("2");

                    total = existe_predial.Total;
                    info = existe_predial.Info;
                    CacheFactura.tipo_factura[referencia[9..24]] = "PREDIAL";
                }

                return ConstruirRespuesta(_, ip, total, info);
            }
            catch (Exception)
            {
                return ErrorResponseConsulta("1");
            }
        }

        private async Task<(bool existe_ica, decimal Total, string Info)> ConsultarIndustria(string referencia, string ip)
        {
            try
            {
                var usuario = referencia.Length >= 10 ? referencia[..10] : referencia;
                var llave = referencia.Length >= 18 ? referencia[..18] : referencia;
                var recibo = referencia.Length >= 24 ? referencia[18..24] : "";

                var resp = await _industria.IYC003R1(
                    new RequestIYC003R1
                    {
                        sesion = "",
                        usuario = usuario,
                        llave = llave,
                        recibo = recibo,
                        validar_rbo = "S",
                    },
                    ip
                );

                if (resp.STATUS != "00")
                    return (false, 0, null);

                var total = _pasarelaServices.CalcularTotalFactura(resp);
                var info = EstructuraPipelineIndustria(resp.MENSAJE);

                return (true, total, info);
            }
            catch (Exception)
            {
                return (false, 0, null);
            }
        }

        private async Task<(bool existe_ica, decimal Total, string Info)> ConsultarPredial(string referencia, string ip)
        {
            try
            {
                //TODO: EN DLL DE BUSQUEDA DE PREDIO Y CONTABILIZACION QUITAR EL USUARIO (CEDULA), ya que no hay manera de obtenerlo, banco no lo envia
                var resp = await _predial.CAT203E(
                    new CAT203ERequest
                    {
                        nro_cat = referencia.Length >= 24 ? referencia[9..24] : referencia,
                        usuario = "17293435",
                        ano = DateTime.Now.Year.ToString(),
                    },
                    ip
                );

                if (resp.STATUS != "00")
                {
                    return (false, 0, null);
                }

                var total = resp.MENSAJE.neto != "" ? decimal.Parse(resp.MENSAJE.neto) : 0;
                var info = EstructuraPipelinePredial(resp.MENSAJE);

                return (true, total, info);
            }
            catch (Exception)
            {
                return (false, 0, null);
            }
        }

        private ConsultaPagoCajaDto ConstruirRespuesta(ConsultaPagoCajaParams _, string ip, decimal total, string info)
        {
            return new ConsultaPagoCajaDto
            {
                Referencia_factura = ip == _optionsMonterrey.Value.IpMonterrey ? _.Referencia_pago : null,
                Valor_referencia_factura = ip == _optionsMonterrey.Value.IpMonterrey ? total.ToString() : null,

                Fecha_limite_pago = DateTime.Now.ToString("dd/MM/yyyy"),
                Descripcion_estado = GenerarEstadoConsulta("0"),
                Valor_factura = _options_zp.Value.IpVillanueva == ip ? total : null,
                Info_Adicional = info,
                Codigo_Estado = "0",
            };
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

        public static string EstructuraPipelineIndustria(MensajeIYC003R1 mensaje)
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

        public static string EstructuraPipelinePredial(CAT203EMensaje mensaje)
        {
            var campos = new[]
            {
                mensaje.nro_cat,
                mensaje.anohasta,
                mensaje.id,
                mensaje.impuesto,
                mensaje.descuento,
                mensaje.interes,
                mensaje.neto,
                mensaje.fecha,
                mensaje.propietario,
                mensaje.direccion,
                mensaje.direcc_noti,
                mensaje.email_noti,
                mensaje.tipo_id_prop,
                mensaje.nit_usunet,
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

                string ip_server_comercio = _pasarelaServices.ObtenerIPComercio(_.Id_Comercio.ToString());

                if (string.IsNullOrEmpty(ip_server_comercio))
                    return ErrorResponseAsiento("2", "E");

                // Validar si ya existe un recaudo con el mismo código y estado A
                bool existe_recaudo = await _context.RECAUDOSBANCOS.AnyAsync(r =>
                    r.codigo_recaudo == _.Referencia_pago
                    && r.codigo_estado_recaudo == EstadoRecaudo.Aprobado
                    && r.codigo_proveedor_recaudo
                        == (comercio_perteneciente == 1 ? ProveedorRecaudo.ZonaPagos : ProveedorRecaudo.BancoBogota)
                );

                if (existe_recaudo)
                    return ErrorResponseAsiento("2", "E");

                //!!!! NO DEPENDER DE COBOL PARA GUARDAR EL DINERO RECAUDADO EN EL BANCO !!!!!
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

                //TODO: Contabilizacion COBOL
                await ContabilizarFactura(_pasarelaServices, ip_server_comercio, _, _context);
            }
            catch (Exception)
            {
                return ErrorResponseAsiento("1", "E");
            }
            return response;
        }
         private static async Task ContabilizarFactura(
            IPasarelaServices pasarelaServices,
            string ip_server_comercio,
            AsientoPagoCajaParams _,
            DataContext _context
        )
        {
            var key = _.Referencia_pago.Length >= 24 ? _.Referencia_pago[9..24] : _.Referencia_pago;

            if (!CacheFactura.tipo_factura.TryGetValue(key, out var tipo))
                return;

            try
            {
                switch (tipo)
                {
                    case "ICA":
                        await pasarelaServices.ContabilizarFacturaAprobadaICA(
                            new RequestIYC006G
                            {
                                sesion = "",
                                usuario = _.Referencia_pago[..10],
                                llave_imp = _.Referencia_pago[..18],
                                recibo = _.Referencia_pago[18..24],
                            },
                            ip_server_comercio
                        );
                        break;

                    default: 
                        await pasarelaServices.ContabilizarFacturaAprobadaPredial(
                            new CAT204GDto
                            {
                                usuario = "",
                                sesion = "",
                                nro_cat = key,
                                ano_fin = DateTime.Now.Year.ToString(),
                                vlr_fac = _.Valor_pagado,
                            },
                            ip_server_comercio
                        );
                        break;
                }

                await MarcarFacturaContabilizada(_, _context);
            }
            finally
            {
                System.Console.WriteLine("ME FUI A LIMPIAR CACHE ❤️❤️");
                CacheFactura.tipo_factura.Remove(key);
            }
        }

        private static async Task MarcarFacturaContabilizada(AsientoPagoCajaParams _, DataContext _context)
        {
            try
            {
                var recaudo = await _context.RECAUDOSBANCOS.FirstOrDefaultAsync(r =>
                    r.codigo_recaudo == _.Referencia_pago && r.codigo_estado_recaudo == EstadoRecaudo.Aprobado
                );

                if (recaudo != null)
                {
                    recaudo.recaudo_contabilizado_ok = true;
                    await _context.SaveChangesAsync();
                }
                System.Console.WriteLine("ME FUI A MARCAR LA FACTURA COMO CONTABILIZADA ❤️❤️");
                
            }
            catch (Exception ex)
            {
                throw new Exception("Error al marcar la factura como contabilizada", ex);
            }
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
