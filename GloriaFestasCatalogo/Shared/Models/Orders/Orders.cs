namespace GloriaFestasCatalogo.Shared.Models.Orders
{
    public class Order
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Whatsapp { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Neighborhood { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public DateTime DataCreated { get; set; } = DateTime.Now;

        public List<OrderCart> Products { get; set; }

    }
}
