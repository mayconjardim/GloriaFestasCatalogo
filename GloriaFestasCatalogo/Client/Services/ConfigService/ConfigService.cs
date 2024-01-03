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

	}
}
