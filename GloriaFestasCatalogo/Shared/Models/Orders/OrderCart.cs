using GloriaFestasCatalogo.Shared.Models.Products;

namespace GloriaFestasCatalogo.Shared.Models.Orders
{
    public class OrderCart
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }

}
