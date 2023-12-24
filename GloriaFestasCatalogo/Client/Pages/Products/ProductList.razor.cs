using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Utils;
using Microsoft.AspNetCore.Components;

namespace GloriaFestasCatalogo.Client.Pages.Products
{
	partial class ProductList
	{

		private ProductResponse products;
		private List<ProductCategoryDto> categories = new List<ProductCategoryDto>();
		private string message = string.Empty;
		private int currentPage = 1;
		private int pageSize = 20;
		private string searchText;
		private int categoryId = 0;

		protected override async Task OnInitializedAsync()
		{
			message = "Carregando Produtos...";

			var result = await ProductService.GetProductsPageableAsync(currentPage, pageSize, categoryId, false, searchText);
			if (!result.Success)
			{
				message = result.Message;
			}
			else
			{
				products = result.Data;
				currentPage = result.Data.CurrentPage;
				await GetCategories();
			}
		}

		private async Task FilterByText()
		{
			var result = await ProductService.GetProductsPageableAsync(currentPage, pageSize, categoryId, false, searchText);

			if (!result.Success)
			{
				message = result.Message;

			}
			else
			{
				products = result.Data;
				currentPage = 1;

			}

			await InvokeAsync(StateHasChanged);
		}

		private async Task FilterByCategory()
		{

			var result = await ProductService.GetProductsPageableAsync(currentPage, pageSize, categoryId, false, searchText);

			if (!result.Success)
			{
				message = result.Message;

			}
			else
			{
				products = result.Data;
				currentPage = 1;
			}

			await InvokeAsync(StateHasChanged);
		}

		private async Task ChangePage(int nextPage)
		{

			var result = await ProductService.GetProductsPageableAsync(nextPage, pageSize, 0, false);

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

		private async Task GetCategories()
		{

			var result = await CategoryService.GetCategoriesAsync();

			if (!result.Success)
			{
			}
			else
			{
				categories = result.Data;
			}
		}

		private async Task HandleCategoryChange(ChangeEventArgs e)
		{

			if (int.TryParse(e.Value.ToString(), out var value))
			{
				categoryId = value;
			}

			await FilterByCategory();
		}

		public async Task AddToCart(ProductDto product)
		{
			await CartService.AddToCart(product);
		}


		private async void UpdateProductNameSearch(ChangeEventArgs e)
		{
			searchText = e.Value.ToString();
			await FilterByText();
		}

		private void NavigateToProductPage(int id)
		{
			NavigationManager.NavigateTo($"/produto/{id}");
		}

	}
}
