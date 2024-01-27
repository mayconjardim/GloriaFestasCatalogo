namespace GloriaFestasCatalogo.Shared.Dtos.Products
{
	public class ProductDto
	{

		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string PhotoUrl { get; set; }
		public List<ProductCategoryDto> Categories { get; set; } = new List<ProductCategoryDto>();
		public List<ProductVariantDto> Variants { get; set; } = new List<ProductVariantDto>();
		public string Tags { get; set; }
		public bool Active { get; set; } = true;

	}
}
