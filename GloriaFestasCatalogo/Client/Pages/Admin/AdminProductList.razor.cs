using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Utils;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace GloriaFestasCatalogo.Client.Pages.Admin
{
	partial class AdminProductList
	{

		private ProductResponse products;
		private ProductDto? selectedProduct;
		private ProductDto newProduct = new ProductDto();
		private string message = string.Empty;
		private int currentPage = 1;
		private int pageSize = 20;
		private string searchText = string.Empty;
		private int categoryId = 0;
		private EditContext _editContext;


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
			}
		}

		private async Task FilterByText()
		{
			var result = await ProductService.GetProductsPageableAsync(currentPage, pageSize, categoryId, searchText);

			if (!result.Success)
			{
				message = result.Message;
				StateHasChanged();
			}
			else
			{
				products = result.Data;
				StateHasChanged();
			}
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

		private async Task CreateProduct()
		{

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

	}
}
