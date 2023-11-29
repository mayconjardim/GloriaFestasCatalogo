using Microsoft.AspNetCore.Components;

namespace GloriaFestasCatalogo.Client.Shared
{
    partial class AdminLayout
    {


        private async Task Logout()
        {
            await LocalStorage.RemoveItemAsync("authToken");
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            NavigationManager.NavigateTo("");
        }

    }
}
