using GloriaFestasCatalogo.Shared.Dtos.Products;

namespace GloriaFestasCatalogo.Shared.Utils
{
    public class ProductResponse
    {

        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }

    }
}
