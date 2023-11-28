using GloriaFestasCatalogo.Shared.Models.Users;
using GloriaFestasCatalogo.Shared.Utils;

namespace GloriaFestasCatalogo.Client.Services.AuthService
{
    public interface IAuthService
    {

        Task<ServiceResponse<string>> Login(UserLogin request);
        Task<bool> IsUserAuthenticated();

    }
}
