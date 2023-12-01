using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GloriaFestasCatalogo.Client.Pages.Admin
{
	partial class AdminProductList
	{

		private ProductResponse products;
		private ProductDto? selectedProduct;
		private ProductCreateDto newProduct = new ProductCreateDto();
		private List<ProductCategoryDto> categories = new List<ProductCategoryDto>();
		private string message = string.Empty;
		private int currentPage = 1;
		private int pageSize = 20;
		private string searchText = string.Empty;
		private int categoryId = 0;

		protected override async Task OnInitializedAsync()
		{
			message = "Carregando Produtos...";

			var result = await ProductService.GetProductsPageableAsync(currentPage, pageSize, categoryId, searchText);
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
			var result = await ProductService.GetProductsPageableAsync(currentPage, pageSize, categoryId, searchText);

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

			var result = await ProductService.GetProductsPageableAsync(currentPage, pageSize, categoryId, searchText);

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

			var result = await ProductService.GetProductsPageableAsync(nextPage, pageSize, categoryId, searchText);

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


		private async Task CreateProduct()
		{

			var result = await ProductService.CreateProduct(newProduct);
			if (!result.Success)
			{
				await InvokeAsync(StateHasChanged);
			}
		}

		private async Task OpenModal(string modal, int id)
		{

			if (id != 0)
			{
				var result = await ProductService.GetProductAsync(id);

				if (!result.Success)
				{
					message = result.Message;
				}
				else
				{
					selectedProduct = result.Data;
				}
			}

			await JSRuntime.InvokeAsync<object>("openModal", modal);
		}

		private async Task CloseModal(string modal)
		{
			await JSRuntime.InvokeAsync<object>("closeModal", modal);
		}

		private async Task HandleSearch()
		{
			await FilterByText();
		}
		private async Task HandleCategoryChange(ChangeEventArgs e)
		{

			if (int.TryParse(e.Value.ToString(), out var value))
			{
				categoryId = value;
			}

			await FilterByCategory();
		}

		private void HandleCreationCategory(ChangeEventArgs e)
		{

			if (int.TryParse(e.Value.ToString(), out var value))
			{
				newProduct.ProductCategoryId = value;
			}

		}

	}
}
