namespace GloriaFestasCatalogo.Shared.Dtos.Products
{
    public class ProductCategoryDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool Active { get; set; } = true;
        public bool IsDragOver { get; set; }

    }
}
