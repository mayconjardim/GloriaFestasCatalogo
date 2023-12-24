using BlazorBootstrap;
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

		[Inject] protected ToastService toastService { get; set; }

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

			await InvokeAsync(() => StateHasChanged());
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

			await InvokeAsync(() => StateHasChanged());
		}

		public async Task ActiveOrDeactiveProduct(int id, bool active)
		{
			ActiveOrDeactive activeOr = new ActiveOrDeactive
			{
				ProductId = id,
				Active = active
			};

			var result = await ProductService.ActiveOrDeactiveProduct(activeOr);

			if (result.Success)
			{
				message = result.Message;
			}
			else
			{
				await InvokeAsync(() => StateHasChanged());
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

			if (newProduct.Name != null)
			{
				var result = await ProductService.CreateProduct(newProduct);

				if (result.Success)
				{
					await InvokeAsync(() =>
					{
						StateHasChanged();
						toastService.Notify(new(ToastType.Success, $"Produto criado com sucesso!"));
					});

					await CloseModal("CreateModal");

					RefreshPage();
				}
				else
				{
					toastService.Notify(new(ToastType.Danger, $"Ocorreu um erro ao criar o produto."));
				}
			}

		}

		private async Task EditProduct()
		{
			if (selectedProduct != null)
			{
				var result = await ProductService.UpdateProduct(selectedProduct);
				if (result.Success)
				{
					await InvokeAsync(() =>
					{
						StateHasChanged();
						toastService.Notify(new(ToastType.Success, $"Produto editado com sucesso!"));
					});

					await CloseModal("EditModal");

					RefreshPage();
				}
				else
				{
					toastService.Notify(new(ToastType.Danger, $"Ocorreu um erro ao editar o produto."));
				}
			}
		}

		private async Task DeleteProduct()
		{
			if (selectedProduct != null)
			{
				var result = await ProductService.DeleteProduct(selectedProduct.Id);

				if (result.Success)
				{
					await InvokeAsync(() =>
					{
						StateHasChanged();
						toastService.Notify(new(ToastType.Success, $"Produto deletado com sucesso!"));
					});

					await CloseModal("DeleteModal");

					RefreshPage();
				}
				else
				{
					toastService.Notify(new(ToastType.Danger, $"Ocorreu um erro ao deletar o produto."));
				}
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

		private async void UpdateProductNameSearch(ChangeEventArgs e)
		{
			searchText = e.Value.ToString();
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

		private void HandleUpdateCategory(ChangeEventArgs e)
		{
			if (int.TryParse(e.Value.ToString(), out var value))
			{
				if (selectedProduct != null)
				{
					selectedProduct.Category.Id = value;
				}
			}
		}

		private async void RefreshPage()
		{
			await Task.Delay(1000);
			NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
		}

	}
}
