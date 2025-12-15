//Migracion de los endpopints de Villanueva tanto ICA como PredialF que estaban realizados en php👌🫓
namespace Aplication.Services.Industria
{
    public interface IIndustria
    {
        //ICA
        Task<ResponseDLLCiudades> DLLCiudades(RequestDLLCiudades request, string direccion_ip_comercio);
        Task<EstructuraResponseDLL> IYC003RP(RequestIYC003RP request, string direccion_ip_comercio);
        Task<EstructuraResponseDLL> IYC003RG(IYC003RGRequest request, string direccion_ip_comercio);
        Task<ResponseIYC003R1> IYC003R1(RequestIYC003R1 request, string direccion_ip_comercio);
        Task<ResponseIYC003RI> IYC003RI(RequestIYC003RI request, string direccion_ip_comercio);
        Task<ResponseIYC003R> IYC003R(RequestIYC003R request, string direccion_ip_comercio);
        Task<ResponseACTDIAN> ACTDIAN(RequestACTDIAN request, string direccion_ip_comercio);
        Task<ResponseIYC002A> IYC002A(RequestIYC002A request, string direccion_ip_comercio);
        Task<ResponseIYC002I> IYC002I(RequestIYC002I request, string direccion_ip_comercio);
        Task<ResponseIYC003> IYC003(RequestIYC003 request, string direccion_ip_comercio);
        Task<ADM005Response> ADM005(ADM005Request request, string direccion_ip_comercio);
        Task<ResponseCODIMP> CODIMP(RequestCODIMP request, string direccion_ip_comercio);
    }

    public class IndustriaServices(IPasarelaServices pasarela_services) : IIndustria
    {
        private readonly IPasarelaServices _pasarelaServices = pasarela_services;

        public async Task<ResponseIYC003> IYC003(RequestIYC003 request, string direccion_ip_comercio)
        {
            return await _pasarelaServices.EjecutarDllGenerica<RequestIYC003, ResponseIYC003>(
                direccion_ip_comercio,
                @"INDUSTRIA_V2/v2/app/impuesto/IYC003.DLL",
                request
            );
        }

        public async Task<ResponseIYC003R> IYC003R(RequestIYC003R request, string direccion_ip_comercio)
        {
            return await _pasarelaServices.EjecutarDllGenerica<RequestIYC003R, ResponseIYC003R>(
                direccion_ip_comercio,
                @"industria_v2/v2/app/impuesto/IYC003R.DLL",
                request
            );
        }

        public async Task<ResponseIYC003R1> IYC003R1(RequestIYC003R1 request, string direccion_ip_comercio)
        {
            return await _pasarelaServices.EjecutarDllGenerica<RequestIYC003R1, ResponseIYC003R1>(
                direccion_ip_comercio,
                @"industria_v2/v2/app/impuesto/IYC003R1.DLL",
                request
            );
        }

        public async Task<EstructuraResponseDLL> IYC003RG(IYC003RGRequest request, string direccion_ip_comercio)
        {
            return await _pasarelaServices.EjecutarDllGenerica<IYC003RGRequest, EstructuraResponseDLL>(
                direccion_ip_comercio,
                @"INDUSTRIA_V2/v2/app/impuesto/IYC003RG.DLL",
                request
            );
        }

        public async Task<EstructuraResponseDLL> IYC003RP(RequestIYC003RP request, string direccion_ip_comercio)
        {
            return await _pasarelaServices.EjecutarDllGenerica<RequestIYC003RP, EstructuraResponseDLL>(
                direccion_ip_comercio,
                @"INDUSTRIA_V2/v2/app/impuesto/IYC003RP.DLL",
                request
            );
        }

        public async Task<ResponseDLLCiudades> DLLCiudades(RequestDLLCiudades request, string direccion_ip_comercio)
        {
            return await _pasarelaServices.EjecutarDllGenerica<RequestDLLCiudades, ResponseDLLCiudades>(
                direccion_ip_comercio,
                @"INDUSTRIA_V2/v2/app/impuesto/CIUD.DLL",
                request
            );
        }

        public async Task<ADM005Response> ADM005(ADM005Request request, string direccion_ip_comercio)
        {
            return await _pasarelaServices.EjecutarDllGenerica<ADM005Request, ADM005Response>(
                direccion_ip_comercio,
                @"INDUSTRIA_V2/v2/app/impuesto/ADM005.DLL",
                request
            );
        }

        public async Task<ResponseIYC002A> IYC002A(RequestIYC002A request, string direccion_ip_comercio)
        {
            return await _pasarelaServices.EjecutarDllGenerica<RequestIYC002A, ResponseIYC002A>(
                direccion_ip_comercio,
                @"INDUSTRIA_V2/v2/app/impuesto/IYC002A.DLL",
                request
            );
        }

        public async Task<ResponseACTDIAN> ACTDIAN(RequestACTDIAN request, string direccion_ip_comercio)
        {
            return await _pasarelaServices.EjecutarDllGenerica<RequestACTDIAN, ResponseACTDIAN>(
                direccion_ip_comercio,
                @"INDUSTRIA_V2/v2/app/impuesto/ACTDIAN.DLL",
                request
            );
        }

        public async Task<ResponseCODIMP> CODIMP(RequestCODIMP request, string direccion_ip_comercio)
        {
            return await _pasarelaServices.EjecutarDllGenerica<RequestCODIMP, ResponseCODIMP>(
                direccion_ip_comercio,
                @"INDUSTRIA_V2/v2/app/impuesto/CODIMP.DLL",
                request
            );
        }

        public async Task<ResponseIYC002I> IYC002I(RequestIYC002I request, string direccion_ip_comercio)
        {
            return await _pasarelaServices.EjecutarDllGenerica<RequestIYC002I, ResponseIYC002I>(
                direccion_ip_comercio,
                @"INDUSTRIA_V2/v2/app/impuesto/IYC002I.DLL",
                request
            );
        }

        public async Task<ResponseIYC003RI> IYC003RI(RequestIYC003RI request, string direccion_ip_comercio)
        {
            return await _pasarelaServices.EjecutarDllGenerica<RequestIYC003RI, ResponseIYC003RI>(
                direccion_ip_comercio,
                @"INDUSTRIA_V2/v2/app/impuesto/IYC003RI.DLL",
                request
            );
        }
    }
}
