using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Models.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GloriaFestasCatalogo.Client.Pages.Admin
{
	partial class AdminCategoryList
	{

		private List<ProductCategoryDto> categories = new List<ProductCategoryDto>();
		private ProductCategoryDto selectedCategorie;
		private ProductCategoryDto newCategory = new ProductCategoryDto();
		private string message = string.Empty;
		private string searchText = string.Empty;

		protected override async Task OnInitializedAsync()
		{
			message = "Carregando Categorias...";

			var result = await CategoryService.GetCategoriesAsync(searchText);
			if (!result.Success)
			{
				message = result.Message;
			}
			else
			{
				categories = result.Data;
			}
		}

		private async Task FilterByText()
		{
			var result = await CategoryService.GetCategoriesAsync(searchText);

			if (!result.Success)
			{
				message = result.Message;

			}
			else
			{
				categories = result.Data;
			}

			await InvokeAsync(StateHasChanged);
		}

		private async Task OpenModal(string modal, int id)
		{

			var result = await CategoryService.GetCategorieAsync(id);

			if (!result.Success)
			{
				message = result.Message;
			}
			else
			{
				selectedCategorie = result.Data;
			}

			await JSRuntime.InvokeAsync<object>("openModal", modal);
		}

		private async Task CloseModal(string modal)
		{
			await JSRuntime.InvokeAsync<object>("closeModal", modal);
		}

		private async Task CreateProduct()
		{

			var result = await CategoryService.CreateCategorie(newCategory);
			if (!result.Success)
			{
				await InvokeAsync(StateHasChanged);
			}
		}

		private async Task EditProduct()
		{
			if (selectedCategorie != null)
			{
				var result = await CategoryService.UpdateCategorie(selectedCategorie);
				if (!result.Success)
				{
					await InvokeAsync(StateHasChanged);

				}
			}
		}

		private async Task DeleteProduct()
		{
			if (selectedCategorie != null)
			{
				var result = await CategoryService.DeleteCategorie(selectedCategorie.Id);
				if (!result.Success)
				{
					await InvokeAsync(StateHasChanged);

				}
			}
		}

		private async void UpdateCategoryNameSearch(ChangeEventArgs e)
		{
			searchText = e.Value.ToString();
			await FilterByText();
		}


	}
}
