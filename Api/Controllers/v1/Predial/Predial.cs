using Aplication.DTOs.Predial;
using Aplication.Services.Predial;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Predial
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [AllowAnonymous]
    public class PredialController(IPredial predial) : ControllerBase
    {
        private readonly IPredial _predial = predial;

        [HttpPost("dll-cat203e")]
        public async Task<ActionResult<CAT203EResponse>> CAT203E(CAT203ERequest request, [FromQuery] string direccion_ip_comercio)
        {
            return await _predial.CAT203E(request, direccion_ip_comercio);
        }    
    }
}