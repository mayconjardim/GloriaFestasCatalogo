﻿using BlazorBootstrap;
using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Utils;
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
        private ProductCategoryDto? draggingModel;

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

            if (newCategory.Name != null)
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
                else
                {
                    toastService.Notify(new(ToastType.Danger, result.Message));
                }
            }
        }
        
        private async void UpdateCategoryNameSearch(ChangeEventArgs e)
        {
            searchText = e.Value.ToString();
            await FilterByText();
        }
        
        private void HandleDrop(ProductCategoryDto landingModel)
        {
            
            if (draggingModel is null) return;
            int originalOrderLanding = landingModel.Order;
            
            categories.Where(x => x.Order >= landingModel.Order).ToList().ForEach(x => x.Order++);
            draggingModel.Order = originalOrderLanding;
            int ii = 0;
            foreach (var model in categories.OrderBy(x=>x.Order).ToList())
            {
                model.Order = ii++;
                model.IsDragOver = false;

                CategoryOrder order = new CategoryOrder() { CategoryId = model.Id, NewOrder = model.Order };
                
                CategoryService.UpdateCategoryOrderAsync(model.Id, order);
            }
        }
        
        public async Task ActiveOrDeactiveCategory(int id, bool active)
        {
            ActiveOrDeactive activeOr = new ActiveOrDeactive
            {
                ProductId = id,
                Active = active
            };

            var result = await CategoryService.ActiveOrDeactiveCategory(activeOr);

            if (result.Success)
            {
                message = result.Message;
            }
            else
            {
                await InvokeAsync(() => StateHasChanged());
            }
        }
        
        private async void RefreshPage()
        {
            await Task.Delay(1000);
            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }

    }
}
