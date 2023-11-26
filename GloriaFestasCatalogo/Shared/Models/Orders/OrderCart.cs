using GloriaFestasCatalogo.Shared.Models.Products;

namespace GloriaFestasCatalogo.Shared.Models.Orders
{
    public class OrderCart
    {

        public Product Product { get; set; }
        public int Quantity { get; set; }

    }
}
