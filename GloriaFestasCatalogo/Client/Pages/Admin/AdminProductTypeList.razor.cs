using BlazorBootstrap;
using GloriaFestasCatalogo.Shared.Dtos.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GloriaFestasCatalogo.Client.Pages.Admin
{
    partial class AdminProductTypeList
    {

        private List<ProductTypeDto> types = new List<ProductTypeDto>();
        private ProductTypeDto selectedType;
        private ProductTypeDto newType = new ProductTypeDto();
        private string message = string.Empty;
        private string searchText = string.Empty;

        [Inject] protected ToastService toastService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            message = "Carregando Tipos de Produto...";

            var result = await ProductTypeService.GetProductTypeAsync(searchText);
            if (!result.Success)
            {
                message = result.Message;
            }
            else
            {
                types = result.Data;
            }
        }

        private async Task FilterByText()
        {
            var result = await ProductTypeService.GetProductTypeAsync(searchText);

            if (!result.Success)
            {
                message = result.Message;

            }
            else
            {
                types = result.Data;
            }

            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenModal(string modal, int id)
        {

            var result = await ProductTypeService.GetProductTypeAsync(id);

            if (!result.Success)
            {
                message = result.Message;
            }
            else
            {
                selectedType = result.Data;
            }

            await JSRuntime.InvokeAsync<object>("openModal", modal);
        }

        private async Task CloseModal(string modal)
        {
            await JSRuntime.InvokeAsync<object>("closeModal", modal);
        }

        private async Task CreateProductType()
        {

            if (newType.Name != null)
            {

                var result = await ProductTypeService.CreateProductType(newType);
                if (result.Success)
                {
                    await InvokeAsync(() =>
                    {
                        StateHasChanged();
                        toastService.Notify(new(ToastType.Success, $"Tipo de produto criado com sucesso!"));
                    });

                    await CloseModal("CreateModal");

                    RefreshPage();
                }
            }

        }

        private async Task EditProductType()
        {
            if (selectedType != null)
            {
                var result = await ProductTypeService.UpdateProductType(selectedType);
                if (result.Success)
                {
                    await InvokeAsync(() =>
                    {
                        StateHasChanged();
                        toastService.Notify(new(ToastType.Success, $"Tipo de produto editado com sucesso!"));
                    });

                    await CloseModal("EditModal");

                    RefreshPage();
                }
            }
        }

        private async Task DeleteProductType()
        {
            if (selectedType != null)
            {
                var result = await ProductTypeService.DeleteProductType(selectedType.Id);
                if (result.Success)
                {
                    await InvokeAsync(() =>
                    {
                        StateHasChanged();
                        toastService.Notify(new(ToastType.Success, $"Tipo de produto deletado com sucesso!"));
                    });

                    await CloseModal("DeleteModal");

                    RefreshPage();

                }
            }
        }

        private async void UpdateTypeNameSearch(ChangeEventArgs e)
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
