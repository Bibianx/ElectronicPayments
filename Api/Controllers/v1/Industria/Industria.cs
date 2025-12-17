using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Industria
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]    
    [AllowAnonymous]
    public class Industria(IIndustria IndustriaServices) : ControllerBase
    {
        private readonly IIndustria _servicesIndustria = IndustriaServices;

        [HttpPost("dll-iyc003")]
        public async Task<ActionResult<ResponseIYC003>> CargarCiudades(RequestIYC003 request, [FromQuery] string direccion_ip_comercio)
        {
            return await _servicesIndustria.IYC003(request, direccion_ip_comercio);
        }

        [HttpPost("dll-ciudades")]
        public async Task<ActionResult<ResponseDLLCiudades>> CargarCiudades(RequestDLLCiudades request, [FromQuery] string direccion_ip_comercio)
        {
            return await _servicesIndustria.DLLCiudades(request, direccion_ip_comercio);
        }

        [HttpPost("dll-iyc003r")]
        public async Task<ActionResult<ResponseIYC003R>> IYC003R(RequestIYC003R _, [FromQuery] string direccion_ip_comercio)
        {
            return await _servicesIndustria.IYC003R(_, direccion_ip_comercio);
        }

        [HttpPost("dll-iyc003r1")]
        public async Task<ActionResult<ResponseIYC003R1>> IYC003R1(RequestIYC003R1 _, [FromQuery] string direccion_ip_comercio)
        {
            return await _servicesIndustria.IYC003R1(_, direccion_ip_comercio);
        }

        [HttpPost("dll-iyc003rg")]
        public async Task<ActionResult<EstructuraResponseDLL>> IYC003RG(IYC003RGRequest _, [FromQuery] string direccion_ip_comercio)
        {
            return await _servicesIndustria.IYC003RG(_, direccion_ip_comercio);
        }

        [HttpPost("dll-iyc003rp")]
        public async Task<ActionResult<EstructuraResponseDLL>> IYC003RP(RequestIYC003RP _, [FromQuery] string direccion_ip_comercio)
        {
            return await _servicesIndustria.IYC003RP(_, direccion_ip_comercio);
        }

        [HttpPost("dll-adm005")]
        public async Task<ActionResult<ADM005Response>> ADM005(ADM005Request _, [FromQuery] string direccion_ip_comercio)
        {
            return await _servicesIndustria.ADM005(_, direccion_ip_comercio);
        }

        [HttpPost("dll-iyc002a")]
        public async Task<ActionResult<ResponseIYC002A>> IYC002A(RequestIYC002A _, [FromQuery] string direccion_ip_comercio)
        {
            return await _servicesIndustria.IYC002A(_, direccion_ip_comercio);
        }

        [HttpPost("dll-actdian")]
        public async Task<ActionResult<ResponseACTDIAN>> ACTDIAN(RequestACTDIAN _, [FromQuery] string direccion_ip_comercio)
        {
            return await _servicesIndustria.ACTDIAN(_, direccion_ip_comercio);
        }

        [HttpPost("dll-codimp")]
        public async Task<ActionResult<ResponseCODIMP>> CODIMP(RequestCODIMP request, [FromQuery] string direccion_ip_comercio)
        {
            return await _servicesIndustria.CODIMP(request, direccion_ip_comercio);
        }

        [HttpPost("dll-iyc002i")]
        public async Task<ActionResult<ResponseIYC002I>> IYC002I(RequestIYC002I _, [FromQuery] string direccion_ip_comercio)
        {
            return await _servicesIndustria.IYC002I(_, direccion_ip_comercio);
        }

        [HttpPost("dll-iyc003ri")]
        public async Task<ActionResult<ResponseIYC003RI>> IYC003RI(RequestIYC003RI _, [FromQuery] string direccion_ip_comercio)
        {
            return await _servicesIndustria.IYC003RI(_, direccion_ip_comercio);
        }
    }
}