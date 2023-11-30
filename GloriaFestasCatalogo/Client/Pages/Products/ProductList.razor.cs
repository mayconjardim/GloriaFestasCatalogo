using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Utils;

namespace GloriaFestasCatalogo.Client.Pages.Products
{
	partial class ProductList
	{

		private ProductResponse products;
		private string message = string.Empty;
		private int currentPage = 1;
		private int pageSize = 20;

		protected override async Task OnInitializedAsync()
		{
			message = "Carregando Produtos...";

			var result = await ProductService.GetProductsPageableAsync(currentPage, pageSize, 0);
			if (!result.Success)
			{
				message = result.Message;
			}
			else
			{
				products = result.Data;
				currentPage = result.Data.CurrentPage;
			}
		}

		private async Task ChangePage(int nextPage)
		{

			var result = await ProductService.GetProductsPageableAsync(nextPage, pageSize, 0);

			if (!result.Success)
			{
				message = result.Message;
			}
			else
			{
				products = result.Data;
				currentPage = result.Data.CurrentPage;
			}
		}

		public async Task AddToCart(ProductDto product)
		{
			await CartService.AddToCart(product);
		}

	}
}
