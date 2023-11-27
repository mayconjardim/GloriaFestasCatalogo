namespace GloriaFestasCatalogo.Shared.Dtos.Orders
{
    public class OrderCartDto
    {

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }

    }
}
