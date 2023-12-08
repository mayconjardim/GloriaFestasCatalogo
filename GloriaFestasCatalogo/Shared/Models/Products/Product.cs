namespace GloriaFestasCatalogo.Shared.Models.Products
{
    public class Product
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public decimal Price { get; set; }
        public ProductCategory Category { get; set; }
        public string Tags { get; set; } = string.Empty;
        public bool Active { get; set; } = true;

    }
}
