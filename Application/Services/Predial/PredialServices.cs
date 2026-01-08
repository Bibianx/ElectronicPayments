using Aplication.DTOs.Predial;

namespace Aplication.Services.Predial
{
    public interface IPredial
    {
        Task<CAT203EResponse> CAT203E(CAT203ERequest request, string direccion_ip_comercio);
    }

    public class PredialServices(IPasarelaServices pasarelaServices) : IPredial
    {
        private readonly IPasarelaServices _pasarelaServices = pasarelaServices;
        public async Task<CAT203EResponse> CAT203E(CAT203ERequest request, string direccion_ip_comercio)
        {
            return await _pasarelaServices.EjecutarDllGenerica<CAT203ERequest, CAT203EResponse>(
                direccion_ip_comercio,
                @"INDUSTRIA_V2/v2/app/predial/CAT203E.DLL",
                request
            );
        }
    }
}