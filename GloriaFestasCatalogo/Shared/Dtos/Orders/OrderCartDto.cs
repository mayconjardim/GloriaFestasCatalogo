using GloriaFestasCatalogo.Shared.Dtos.Products;

namespace GloriaFestasCatalogo.Shared.Dtos.Orders
{
    public class OrderCartDto
    {

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }

    }
}
