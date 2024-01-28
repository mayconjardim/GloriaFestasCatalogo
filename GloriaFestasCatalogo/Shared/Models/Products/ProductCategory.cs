namespace GloriaFestasCatalogo.Shared.Models.Products
{
    public class ProductCategory
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool Active { get; set; } = true;
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
