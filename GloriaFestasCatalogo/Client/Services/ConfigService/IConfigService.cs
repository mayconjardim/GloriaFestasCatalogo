using GloriaFestasCatalogo.Shared.Dtos.Config;
using GloriaFestasCatalogo.Shared.Utils;

namespace GloriaFestasCatalogo.Client.Services.ConfigService
{
	public interface IConfigService
	{

		Task<ServiceResponse<AppConfigDto>> GetConfig();

	}
}
