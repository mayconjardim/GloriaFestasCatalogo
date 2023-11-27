using GloriaFestasCatalogo.Client.Services.CartService;
using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Utils;
using Microsoft.AspNetCore.Components;

namespace GloriaFestasCatalogo.Client.Shared
{
    partial class NavMenu
    {
        private int cartCount = 0;
        private List<CartProduct> cart = new List<CartProduct>();
        decimal subtotal = 0;
        private bool IsButtonDisabled = true;

        protected override async Task OnInitializedAsync()
        {
            cart = await CartService.GetCart();

            if (cart != null)
            {
                cartCount = cart.Count();
                IsButtonDisabled = false;

                foreach (var cartProducts in cart)
                {
                    subtotal += cartProducts.Product.Price * cartProducts.Quantity;
                }
            }

            CartService.CartUpdated += HandleCartUpdated;
        }

        private async Task UpdateQuantity(bool isAdding, ProductDto product)
        {
            int quantityChange = isAdding ? 1 : -1;

            await CartService.UpdateCartItemQuantity(product, quantityChange);

            UpdateCartInfo();
        }

        private void HandleCartUpdated(object sender, EventArgs e)
        {
            UpdateCartInfo();
        }

        private async void UpdateCartInfo()
        {
            var updatedCart = await CartService.GetCart();
            cartCount = updatedCart?.Count() ?? 0;
            cart = updatedCart;

            subtotal = 0;
            foreach (var cartProducts in updatedCart)
            {
                subtotal += cartProducts.Product.Price * cartProducts.Quantity;
            }

            IsButtonDisabled = cartCount == 0;

            var currentUrl = NavigationManager.Uri;
            await InvokeAsync(StateHasChanged);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                CartService.CartUpdated += HandleCartUpdated;
            }
        }

        private async Task NavigateToHome()
        {
            NavigationManager.NavigateTo("/");
        }

        private async Task NavigateToOrder()
        {
            var currentUrl = NavigationManager.Uri;

            if (currentUrl.Contains("order", StringComparison.OrdinalIgnoreCase))
            {
                IsButtonDisabled = true;
            }
            else if (!IsButtonDisabled)
            {

                NavigationManager.NavigateTo(currentUrl + "order");
            }

        }
    }
}
