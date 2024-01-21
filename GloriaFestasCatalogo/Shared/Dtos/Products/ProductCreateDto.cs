namespace GloriaFestasCatalogo.Shared.Dtos.Products
{
	public class ProductCreateDto
	{

		public string Name { get; set; }
		public string Description { get; set; } = string.Empty;
		public string PhotoUrl { get; set; }
		public int? ProductCategoryId { get; set; }
		public string Tags { get; set; } = string.Empty;
		public List<ProductVariantDto> Variants { get; set; } = new List<ProductVariantDto>();
		public bool Active { get; set; } = true;

	}
}
