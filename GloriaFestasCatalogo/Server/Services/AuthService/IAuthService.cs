using GloriaFestasCatalogo.Shared.Utils;

namespace GloriaFestasCatalogo.Server.Services.AuthService
{
    public interface IAuthService
    {

        Task<ServiceResponse<string>> Login(string email, string password);

    }
}
