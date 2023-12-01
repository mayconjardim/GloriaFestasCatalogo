namespace GloriaFestasCatalogo.Shared.Dtos.Products
{
	public class ProductDto
	{

		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string PhotoUrl { get; set; }
		public decimal Price { get; set; }
		public ProductCategoryDto Category { get; set; }
		public bool Active { get; set; } = true;

	}
}
