using GloriaFestasCatalogo.Client.Shared;
using GloriaFestasCatalogo.Shared.Dtos.Config;
using GloriaFestasCatalogo.Shared.Dtos.Orders;
using GloriaFestasCatalogo.Shared.Utils;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace GloriaFestasCatalogo.Client.Pages.Orders
{
	partial class Order
	{

		private List<CartProduct> cart = new List<CartProduct>();
		private OrderCreateDto order = new OrderCreateDto();
		private AppConfigDto appConfig;
		decimal subtotal = 0;
		private EditContext _editContext;
		string message;

		protected override async Task OnInitializedAsync()
		{
			var config = await ConfigService.GetConfig();
			if (!config.Success)
			{
				message = config.Message;
			}
			else
			{
				appConfig = config.Data;
			}

			_editContext = new EditContext(order);
			cart = await CartService.GetCart();
			CartService.CartUpdated += HandleCartUpdated;
			foreach (var cartProducts in cart)
			{
				subtotal += cartProducts.Product.Variant.Price * cartProducts.Quantity;
			}
		}

		private void HandleCartUpdated(object sender, EventArgs e)
		{
			UpdateCartInfo();
		}

		private async void UpdateCartInfo()
		{
			var updatedCart = await CartService.GetCart();
			cart = updatedCart;

			subtotal = 0;
			foreach (var cartProducts in updatedCart)
			{
				subtotal += cartProducts.Product.Variant.Price * cartProducts.Quantity;
			}

			await InvokeAsync(StateHasChanged);
		}


		private async Task ProcessOrders()
		{
			order.TotalPrice = subtotal;
			order.Products = cart;

			var response = await OrderService.CreateOrder(order);
			if (response.Success)
			{

				if (_editContext.Validate() && response.Data != null)
				{
					var OrderId = response.Data.Id;
					var orderReponse = await OrderService.GetOrderById(OrderId);
					await CartService.CleanCart();
					NavigationManager.NavigateTo("/");

					if (orderReponse.Data != null)
					{
						var returnOrder = orderReponse.Data;

						await JS.InvokeVoidAsync("window.open", GetWhatsAppURL(appConfig.PhoneNumber, AppConstants.Message(returnOrder)));
					}

					RefreshPage();
				}

			}

		}

		private string GetWhatsAppURL(string phoneNumber, string message)
		{
			var encodedMessage = Uri.EscapeDataString(message);
			return $"https://wa.me/{phoneNumber}?text={encodedMessage}";
		}

		private void RefreshPage()
		{
			NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
		}
	}
}
