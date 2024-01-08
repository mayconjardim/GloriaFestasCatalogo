using AutoMapper;
using GloriaFestasCatalogo.Server.Data;
using GloriaFestasCatalogo.Shared.Dtos.Config;
using GloriaFestasCatalogo.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace GloriaFestasCatalogo.Server.Services.ConfigService
{
    public class ConfigService : IConfigService
    {

        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ConfigService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<AppConfigDto>> GetConfig()
        {

            var response = new ServiceResponse<AppConfigDto>();
            var config = await _context.AppConfig.FirstOrDefaultAsync();

            if (config == null)
            {
                response.Success = false;
                response.Message = $"A configuração não existe!";
            }
            else
            {
                response.Data = _mapper.Map<AppConfigDto>(config);
            }

            return response;

        }

        public async Task<ServiceResponse<AppConfigDto>> UpdateConfig(AppConfigDto config)
        {
            try
            {

                var appConfig = await _context.AppConfig.FindAsync(config.Id);

                if (appConfig == null)
                {
                    return new ServiceResponse<AppConfigDto>
                    {
                        Success = false,
                        Message = "Config não encontrados."
                    };
                }

                appConfig.PhoneNumber = config.PhoneNumber;
              
                _context.Update(appConfig);
                await _context.SaveChangesAsync();

                return new ServiceResponse<AppConfigDto>
                {
                    Success = true,
                    Data = _mapper.Map<AppConfigDto>(appConfig)
                };
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
