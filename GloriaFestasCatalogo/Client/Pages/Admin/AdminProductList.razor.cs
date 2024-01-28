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
        private ProductCreateDto? newProduct = new ProductCreateDto();
        private List<ProductCategoryDto> categories = new List<ProductCategoryDto>();
        private List<ProductTypeDto> productTypes = new List<ProductTypeDto>();
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
                await GetProductTypes();
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

        private async Task GetProductTypes()
        {

            var result = await ProductTypeService.GetProductTypeAsync();

            if (!result.Success)
            {
            }
            else
            {
                productTypes = result.Data;
            }
        }



        private async Task CreateProduct()
        {

            if (IsValid())
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

                if (IsValidEdit())
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
            selectedProduct = null;
            newProduct = new ProductCreateDto();
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
                //newProduct.ProductCategoryId = value;
            }
        }

        private void HandleUpdateCategory(ChangeEventArgs e)
        {

            if (int.TryParse(e.Value.ToString(), out var value))
            {
                //selectedProduct.Category.Id = value;
            }
        }

        private void AddVariant(int type)
        {

            if (type == 0)
            {
                newProduct.Variants
                    .Add(new ProductVariantDto { });
            }
            if (type == 1)
            {
                selectedProduct.Variants
                    .Add(new ProductVariantDto { });
            }

        }

        private void RemoveVariant(ProductVariantDto productVariantDto, int type)
        {
            if (type == 0)
            {
                newProduct.Variants.Remove(productVariantDto);
            }
            if (type == 1)
            {
                selectedProduct.Variants.Remove(productVariantDto);
            }
        }
		
        private void AddCategorie(int type)
        {

            if (type == 0)
            {
                newProduct.Categories
                    .Add(new ProductCategoryDto() { });
            }
            if (type == 1)
            {
                selectedProduct.Categories
                    .Add(new ProductCategoryDto { });
            }

        }
		
        private void RemoveCategorie(ProductCategoryDto categoryDto, int type)
        {
            if (type == 0)
            {
                newProduct.Categories.Remove(categoryDto);
            }
            if (type == 1)
            {
                selectedProduct.Categories.Remove(categoryDto);
            }
        }
		

        private void SelectProductType(int productTypeId, ProductVariantDto variant)
        {
		
            variant.ProductTypeId = productTypeId;
        }

        private void SelectCategorieType(int newCategoryID, ProductCategoryDto newCategoryDto, int type)
        {
            var newCategory = categories.FirstOrDefault(c => c.Id == newCategoryID);

            if (type == 0)
            {
                if (newCategory != null)
                {
                    int index = newProduct.Categories.FindIndex(c => c == newCategoryDto);

                    if (index != -1)
                    {
                        newProduct.Categories[index] = newCategory;
                    }
                }
            }

            if (type == 1)
            {
                if (newCategory != null)
                {
                    int index = selectedProduct.Categories.FindIndex(c => c == newCategoryDto);

                    if (index != -1)
                    {
                        selectedProduct.Categories[index] = newCategory;
                    }
                }
            }
        }
		
        bool IsValid()
        {

            if (string.IsNullOrEmpty(newProduct.Name) || string.IsNullOrEmpty(newProduct.Description) || string.IsNullOrEmpty(newProduct.PhotoUrl))
            {
                toastService.Notify(new(ToastType.Danger, $"Preencha todos campos necessarios!"));
                return false;
            }

            if (newProduct.Variants.Count == 0)
            {
                toastService.Notify(new(ToastType.Danger, $"É necessario adicionar um tipo de produto!"));
                return false;
            }

            if (newProduct.Categories.Count <=0)
            {
                toastService.Notify(new(ToastType.Danger, $"É necessario adicionar uma categoria ao produto!"));
                return false;
            }

            else
            {

                if (newProduct.Variants.Any(variant => variant.ProductTypeId == 0))
                {
                    toastService.Notify(new(ToastType.Danger, $"É necessario adicionar um tipo de produto!"));
                    return false;
                }

                if (newProduct.Variants.Any(variant => variant.Price == 0.00m))
                {
                    toastService.Notify(new(ToastType.Danger, $"É necessario adicionar um valor maior que 0!"));
                    return false;
                }
            }

            var duplicateProductTypeIds = newProduct.Variants
                .GroupBy(v => v.ProductTypeId)
                .Any(g => g.Count() > 1);

            if (duplicateProductTypeIds)
            {
                toastService.Notify(new(ToastType.Danger, $"Existem tipos de produto repetidos! "));
                return false;
            }

            return true;
        }

        bool IsValidEdit()
        {

            if (string.IsNullOrEmpty(selectedProduct.Name) || string.IsNullOrEmpty(selectedProduct.Description) || string.IsNullOrEmpty(selectedProduct.PhotoUrl))
            {
                toastService.Notify(new(ToastType.Danger, $"Preencha todos campos necessarios!"));
                return false;
            }
			
            if (selectedProduct.Categories.Count <= 0)
            {
                toastService.Notify(new(ToastType.Danger, $"É necessario adicionar uma categoria ao produto!"));
                return false;
            }
            else
            {
                if (selectedProduct.Categories.Any(c => c.Id == 0))
                {
                    toastService.Notify(new(ToastType.Danger, $"Você adicionou um campo de categoria, mas não " +
                                                              $"selecionou uma opção! Por favor, escolha uma categoria."));
                    return false;
                }
                
                var duplicateCategoriesIds = selectedProduct.Categories
                    .GroupBy(v => v.Id)
                    .Any(g => g.Count() > 1);

                if (duplicateCategoriesIds)
                {
                    toastService.Notify(new(ToastType.Danger, $"Ops! Parece que você adicionou categorias duplicadas. " +
                                                              $"Por favor, verifique e remova categorias repetidas."));
                    return false;
                }
                
            }
            
            if (selectedProduct.Variants.Count == 0)
            {
                toastService.Notify(new(ToastType.Danger, $"É necessario adicionar um tipo de produto!"));
                return false;
            }
            else
            {

                if (selectedProduct.Variants.Any(variant => variant.ProductTypeId == 0))
                {
                    toastService.Notify(new(ToastType.Danger, $"É necessario adicionar um tipo de produto!"));
                    return false;
                }

                if (selectedProduct.Variants.Any(variant => variant.Price == 0.00m))
                {
                    toastService.Notify(new(ToastType.Danger, $"É necessario adicionar um valor maior que 0!"));
                    return false;
                }
            }

            var duplicateProductTypeIds = selectedProduct.Variants
                .GroupBy(v => v.ProductTypeId)
                .Any(g => g.Count() > 1);

            if (duplicateProductTypeIds)
            {
                toastService.Notify(new(ToastType.Danger, $"Existem tipos de produto repetidos! "));
                return false;
            }

            return true;
        }
        
        private async void RefreshPage()
        {
            await Task.Delay(1000);
            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }

    }
}