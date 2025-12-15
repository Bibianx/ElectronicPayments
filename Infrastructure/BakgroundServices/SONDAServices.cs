using Aplication.DTOs.ZonaPagos;
using Aplication.Services.ZonaPagos;
using Common;
using Domain.Entities.ZonaPagos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;

namespace Infraestructure.BackgroundServices
{
    public class SONDAServices(IServiceScopeFactory scopeFactory, ILogger<SONDAServices> logger, IOptions<ClavesPSE> opcionesZonaPagos)
        : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly ClavesPSE _claves = opcionesZonaPagos.Value;
        private readonly ILogger<SONDAServices> _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
                await ConsultarPagosPendientes(stoppingToken);
            }
        }

        private async Task ConsultarPagosPendientes(CancellationToken token)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var zonaPagoService = scope.ServiceProvider.GetRequiredService<IZonaPagoPSE>();
            var helpers = scope.ServiceProvider.GetRequiredService<IPasarelaServices>();

            try
            {
                var pagos_pendientes = await context
                    .HISTORIALZP.Include(p => p.intentos_zp)
                    .Where(p => p.descrip_estado_fin.Equals("PENDIENTE"))
                    .OrderBy(p => p.fecha_ini)
                    .ThenBy(p => p.hora_ini)
                    .ToListAsync(token);

                if (pagos_pendientes.Count == 0)
                {
                    System.Console.WriteLine("NO HAY PAGOS PENDIENTES PARA PROCESAR");
                    _logger.LogInformation("No hay pagos pendientes para procesar");
                    return;
                }

                foreach (var lote in pagos_pendientes.Chunk(20))
                {
                    foreach (var item in lote)
                    {
                        try
                        {
                            await ProcesarPago(item, zonaPagoService, helpers);
                            await context.SaveChangesAsync(token);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error individual procesando el pago {PagoId}", item.intentos_zp?.str_id_pago ?? "SIN ID");

                            item.fecha_fin = DateOnly.FromDateTime(DateTime.Now);
                            item.hora_fin = TimeOnly.FromDateTime(DateTime.Now);
                            item.descrip_estado_fin = "RECHAZADO POR EXCEPCIÓN";
                            item.intentos_zp.estado_intento = "E";
                            item.origen_cambio = "SONDA";
                            item.cod_estado_fin = "1000";

                            await context.SaveChangesAsync(token);
                        }
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1), token);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error general durante el proceso de pagos: {Message}", ex.Message);
            }
        }

        private async Task ProcesarPago(HISTORIALZP item, IZonaPagoPSE zonaPagoService, IPasarelaServices helpers)
        {
            try
            {
                bool verificado = await VerificarPago(item, zonaPagoService, helpers);
                if (verificado)
                {
                    var estado = item.descrip_estado_fin == "PENDIENTE" ? "SIGUE PENDIENTE" : "FINALIZADO";
                    _logger.LogInformation("intentos_zp {PagoId} procesado - {Estado}", item.intentos_zp?.str_id_pago ?? "SIN ID", estado);
                }
                else
                {
                    _logger.LogWarning("No se pudo verificar el pago {PagoId}", item.intentos_zp?.str_id_pago ?? "SIN ID");
                    item.fecha_fin = DateOnly.FromDateTime(DateTime.Now);
                    item.hora_fin = TimeOnly.FromDateTime(DateTime.Now);
                    item.descrip_estado_fin = "RECHAZADO POR ERROR";
                    item.intentos_zp.estado_intento = "E";
                    item.origen_cambio = "SONDA";
                    item.cod_estado_fin = "1000";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar pago {PagoId}: {Message}", item.intentos_zp?.str_id_pago ?? "SIN ID", ex.Message);
                item.fecha_fin = DateOnly.FromDateTime(DateTime.Now);
                item.hora_fin = TimeOnly.FromDateTime(DateTime.Now);
                item.descrip_estado_fin = "RECHAZADO POR EXCEPCIÓN";
                item.intentos_zp.estado_intento = "E";
                item.cod_estado_fin = "ERR_EXC";
                item.origen_cambio = "SONDA";
            }
        }

        private async Task<bool> VerificarPago(HISTORIALZP pago, IZonaPagoPSE zonaPagoService, IPasarelaServices helpers)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pago.intentos_zp?.str_id_pago))
                {
                    _logger.LogWarning("Pago sin ID válido. No se puede verificar.");
                    return false;
                }

                var pago_verificado = await zonaPagoService.VerificarPago(
                    new VerificarPagoParams
                    {
                        int_id_comercio = _claves.IntIdComercio,
                        str_usr_comercio = _claves.StrUsrComercio,
                        str_pwd_Comercio = _claves.StrPwdComercio,
                        str_id_pago = pago.intentos_zp.str_id_pago,
                        int_no_pago = -1,
                    }
                );

                if (pago_verificado == null)
                {
                    _logger.LogWarning("Verificación fallida: respuesta nula para pago {PagoId}", pago.intentos_zp.str_id_pago);
                    return false;
                }

                pago.origen_cambio = "SONDA";
                await helpers.ActualizarEstadoPago(pago, pago_verificado);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar pago {PagoId}: {Message}", pago.intentos_zp?.str_id_pago ?? "SIN ID", ex.Message);
                return false;
            }
        }
    }
}
