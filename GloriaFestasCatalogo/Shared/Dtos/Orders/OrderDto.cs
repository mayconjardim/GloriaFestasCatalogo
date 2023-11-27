namespace GloriaFestasCatalogo.Shared.Dtos.Orders
{
    public class OrderDto
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Whatsapp { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string Neighborhood { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Complement { get; set; } = string.Empty;
        public string Observation { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public List<OrderCartDto> Products { get; set; }
        public DateTime OrderDate { get; set; }

    }
}
