using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Utils;


namespace GloriaFestasCatalogo.Client.Services.CartService
{
	public class CartService
	{

		private readonly ILocalStorageService _localStorage;

		public CartService(ILocalStorageService localStorage)
		{
			_localStorage = localStorage;

		}

		public event EventHandler CartUpdated;

		public async Task<List<CartProduct>> GetCart()
		{
			return await _localStorage.GetItemAsync<List<CartProduct>>("cart");
		}

		public async Task AddToCart(ProductCartDto product)
		{
			var cart = await _localStorage.GetItemAsync<List<CartProduct>>("cart");

			if (cart == null)
			{
				cart = new List<CartProduct>();
			}

			CartProduct existingCartItem = cart.FirstOrDefault(cp => cp.Product.Id == product.Id && cp.Product.Variant.ProductTypeId == product.Variant.ProductTypeId);

			if (existingCartItem != null)
			{
				existingCartItem.Quantity++;
			}
			else
			{
				CartProduct cartProduct = new CartProduct
				{
					Product = product,
					Quantity = 1
				};

				cart.Add(cartProduct);
			}

			await _localStorage.SetItemAsync("cart", cart);
			CartUpdated?.Invoke(this, EventArgs.Empty);
		}

		public async Task UpdateCartItemQuantity(ProductCartDto product, int quantityChange)
		{
			var cart = await _localStorage.GetItemAsync<List<CartProduct>>("cart");

			if (cart == null)
			{
				return;
			}

			CartProduct existingCartItem = cart.FirstOrDefault(cp => cp.Product.Id == product.Id && cp.Product.Variant.ProductTypeId == product.Variant.ProductTypeId);

			if (existingCartItem != null)
			{
				existingCartItem.Quantity += quantityChange;

				if (existingCartItem.Quantity <= 0)
				{
					cart.Remove(existingCartItem);
				}

				await _localStorage.SetItemAsync("cart", cart);
				CartUpdated?.Invoke(this, EventArgs.Empty);
			}
		}

		public async Task CleanCart()
		{
			await _localStorage.ClearAsync();
		}

	}
}
