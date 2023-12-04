using GloriaFestasCatalogo.Shared.Dtos.Products;
using Microsoft.AspNetCore.Components;

namespace GloriaFestasCatalogo.Client.Pages.Products
{
	partial class ProductDetail
	{

		private ProductDto product = null;
		private string message = string.Empty;

		[Parameter]
		public int Id { get; set; }

		protected override async Task OnInitializedAsync()
		{
			message = "Carregando Produto...";

			var result = await ProductService.GetProductAsync(Id);
			if (!result.Success)
			{
				message = result.Message;
			}
			else
			{
				product = result.Data;
			}
		}

		public async Task AddToCart(ProductDto product)
		{
			await CartService.AddToCart(product);
		}


	}
}
