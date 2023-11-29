using GloriaFestasCatalogo.Shared.Models.Users;

namespace GloriaFestasCatalogo.Client.Shared
{
    partial class Login
    {

        private UserLogin user = new UserLogin();

        private string errorMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/admin/pedidos");
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
                NavigationManager.NavigateTo("/admin/pedidos");
            }
            else
            {
                errorMessage = result.Message;
            }
        }

    }
}
