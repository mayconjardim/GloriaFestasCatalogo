using GloriaFestasCatalogo.Shared.Models.Users;
using Microsoft.AspNetCore.WebUtilities;

namespace GloriaFestasCatalogo.Client.Shared
{
    partial class Login
    {

        private UserLogin user = new UserLogin();

        private string errorMessage = string.Empty;

        private string returnUrl = string.Empty;

        protected override void OnInitialized()
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("returnUrl", out var url))
            {
                returnUrl = url;
            }
        }

        private async Task HandleLogin()
        {
            var result = await AuthService.Login(user);
            if (result.Success)
            {
                errorMessage = string.Empty;

                await LocalStorage.SetItemAsync("authToken", result.Data);
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                NavigationManager.NavigateTo(returnUrl);
            }
            else
            {
                errorMessage = result.Message;
            }
        }

    }
}
