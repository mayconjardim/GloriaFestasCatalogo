using BlazorBootstrap;
using GloriaFestasCatalogo.Shared.Dtos.Products;
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

		[Inject] protected ToastService toastService { get; set; }

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

		private async Task CreateCategorie()
		{

			var result = await CategoryService.CreateCategorie(newCategory);
			if (result.Success)
			{
				await InvokeAsync(() =>
				{
					StateHasChanged();
					toastService.Notify(new(ToastType.Success, $"Categoria criada com sucesso!"));
				});

				await CloseModal("CreateModal");

				RefreshPage();
			}
		}

		private async Task EditCategorie()
		{
			if (selectedCategorie != null)
			{
				var result = await CategoryService.UpdateCategorie(selectedCategorie);
				if (result.Success)
				{
					await InvokeAsync(() =>
					{
						StateHasChanged();
						toastService.Notify(new(ToastType.Success, $"Categoria editada com sucesso!"));
					});

					await CloseModal("EditModal");

					RefreshPage();
				}
			}
		}

		private async Task DeleteCategorie()
		{
			if (selectedCategorie != null)
			{
				var result = await CategoryService.DeleteCategorie(selectedCategorie.Id);
				if (result.Success)
				{
					await InvokeAsync(() =>
					{
						StateHasChanged();
						toastService.Notify(new(ToastType.Success, $"Categoria deletada com sucesso!"));
					});

					await CloseModal("DeleteModal");

					RefreshPage();

				}
			}
		}

		private async void UpdateCategoryNameSearch(ChangeEventArgs e)
		{
			searchText = e.Value.ToString();
			await FilterByText();
		}

		private async void RefreshPage()
		{
			await Task.Delay(1000);
			NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
		}

	}
}
