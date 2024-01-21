using GloriaFestasCatalogo.Shared.Dtos.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GloriaFestasCatalogo.Client.Pages.Products
{
	partial class ProductDetail
	{

		private ProductDto product = null;
		private ProductCartDto cartProduct = new ProductCartDto();
		private string message = string.Empty;
		private int currentTypeId = 1;

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

		private ProductVariantDto GetSelectedVariant()
		{
			var variant = product.Variants.FirstOrDefault(v => v.ProductTypeId == currentTypeId);
			return variant;
		}

		public async Task AddToCart(ProductDto product)
		{
			var productVariant = GetSelectedVariant();

			cartProduct.Id = product.Id;
			cartProduct.Name = product.Name;
			cartProduct.Description = product.Description;
			cartProduct.PhotoUrl = product.PhotoUrl;
			cartProduct.Category = product.Category;
			cartProduct.Tags = product.Tags;
			cartProduct.Variant = productVariant;
			cartProduct.ProductTypeName = productVariant.ProductTypeName;

			await CartService.AddToCart(cartProduct);
		}


	}
}
