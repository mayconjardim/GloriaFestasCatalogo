using GloriaFestasCatalogo.Shared.Utils;

namespace GloriaFestasCatalogo.Client.Pages.Products
{
    partial class ProductList
    {

        private ProductResponse products;
        private string message = string.Empty;
        private int page = 1;
        private int pageSize = 20;

        protected override async Task OnInitializedAsync()
        {
            message = "Carregando Produtos...";

            var result = await ProductService.GetProductsPageableAsync(page, pageSize);
            if (!result.Success)
            {
                message = result.Message;
            }
            else
            {
                products = result.Data;
                page = result.Data.CurrentPage;
            }
        }

    }
}
