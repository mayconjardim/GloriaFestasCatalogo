using GloriaFestasCatalogo.Shared.Dtos.Config;
using GloriaFestasCatalogo.Shared.Utils;

namespace GloriaFestasCatalogo.Server.Services.ConfigService
{
    public interface IConfigService
    {

        Task<ServiceResponse<AppConfigDto>> GetConfig();
        Task<ServiceResponse<AppConfigDto>> UpdateConfig(AppConfigDto config);

    }
}
