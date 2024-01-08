using GloriaFestasCatalogo.Shared.Dtos.Config;
using GloriaFestasCatalogo.Shared.Utils;
using System.Net.Http.Json;

namespace GloriaFestasCatalogo.Client.Services.ConfigService
{
    public class ConfigService : IConfigService
    {

        private readonly HttpClient _http;

        public ConfigService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ServiceResponse<AppConfigDto>> GetConfig()
        {
            return await _http.GetFromJsonAsync<ServiceResponse<AppConfigDto>>($"api/config/");
        }

        public async Task<ServiceResponse<AppConfigDto>> UpdateConfig(AppConfigDto config)
        {
            try
            {
                var result = await _http.PutAsJsonAsync($"api/config/{config.Id}", config);
                return await result.Content.ReadFromJsonAsync<ServiceResponse<AppConfigDto>>();
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AppConfigDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
