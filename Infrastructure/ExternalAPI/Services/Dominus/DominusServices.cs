using Infrastructure.ExternalAPI.Common.Response;
using Infraestructure.ExternalAPI.DTOs.Dominus;
using Microsoft.AspNetCore.WebUtilities;
using Aplication.Interfaces.Dominus;
using System.Text.Json;

namespace Infraestructure.ExternalAPI.Services.Dominus
{
    public sealed class DominusServices(IHttpClientFactory httpClientFactory) : IDominus
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private static string token_cache;
        public async Task<ServiceResponse<TokenResponse>> GenerarToken(TokenParams request)
        {
            var response = new ServiceResponse<TokenResponse>();
            try
            {
                var cliente = _httpClientFactory.CreateClient("Dominus");

                var consulta_api_dominus = await cliente.PostAsJsonAsync("oauth/v2/token", request);
                if (!consulta_api_dominus.IsSuccessStatusCode)
                {
                    response.success = false;
                    response.message = "Error al comunicarse con el servicio Dominus";
                    return response;
                }

                var contenido_respuesta = await consulta_api_dominus.Content.ReadFromJsonAsync<TokenResponse>();
                token_cache = contenido_respuesta.access_token;
                
                response.success = true;
                response.message = "Token generado exitosamente";
                response.data = new TokenResponse
                {
                    access_token = contenido_respuesta.access_token,
                    expires_in = contenido_respuesta.expires_in,
                    token_type = contenido_respuesta.token_type
                };
            }
            catch (Exception ex)
            {
                response.success = false;
                response.error = $"Excepción al comunicarse con el servicio Dominus {ex.Message}";
                return response;
                
            }
            return response;
        }

        public async Task<ServiceResponse<ResponseListadoConsolidados>> ConsultarListadoConsolidados(RequestListadoConsolidados request)
        {
            var response = new ServiceResponse<ResponseListadoConsolidados>();
            try
            {
                var cliente = _httpClientFactory.CreateClient("Dominus");
                cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token_cache);

                var queryParams = new Dictionary<string, string>
                {
                    ["branch_id"] = request.branch_id.ToString(),
                    ["start_date"] = request.start_date.ToString(),
                    ["final_date"] = request.final_date.ToString()
                };

                var url = QueryHelpers.AddQueryString(
                    "integrations/administrative/dominus/consolidatedlist",
                    queryParams
                );

                var httpResponse = await cliente.GetAsync(url);
                var content_response = await httpResponse.Content.ReadAsStringAsync();

                if (!httpResponse.IsSuccessStatusCode)
                {
                    response.success = false;
                    response.message = "Error al comunicarse con el servicio Dominus";
                    response.error = content_response;
                    return response;
                }

                var contenido = JsonSerializer.Deserialize<ResponseListadoConsolidados>(content_response);

                response.success = true;
                response.message = "Consulta exitosa";
                response.data = contenido;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.error = $"Excepción al comunicarse con Dominus: {ex.Message}";
            }

            return response;
        }

      
        public async Task<ServiceResponse<ConsultaVentasConsolidadoResponse>> ConsultarVentasConsolidado(ConsultaVentasConsolidadoParams request)
        {
            var response = new ServiceResponse<ConsultaVentasConsolidadoResponse>();
            try
            {
                var cliente = _httpClientFactory.CreateClient("Dominus");
                cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token_cache);

                var queryParams = new Dictionary<string, string>
                {
                    ["consolidated_id"] = request.consolidated_id,
                };

                var url = QueryHelpers.AddQueryString(
                    "integrations/administrative/dominus/consolidated",
                    queryParams
                );

                var httpResponse = await cliente.GetAsync(url);
                var content_response = await httpResponse.Content.ReadAsStringAsync();

                if (!httpResponse.IsSuccessStatusCode)
                {
                    response.success = false;
                    response.message = "Error al comunicarse con el servicio Dominus";
                    response.error = content_response;
                    return response;
                }
                var contenido = JsonSerializer.Deserialize<ConsultaVentasConsolidadoResponse>(content_response);
                response.success = true;
                response.message = "Consulta exitosa";
                response.data = contenido;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.error = $"Excepción al comunicarse con el servicio Dominus {ex.Message}";
                return response;
                
            }
            return response;
        }
    }
    
}