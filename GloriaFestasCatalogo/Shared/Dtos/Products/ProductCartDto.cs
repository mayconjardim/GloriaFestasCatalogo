namespace GloriaFestasCatalogo.Shared.Dtos.Products
{
	public class ProductCartDto
	{

		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string PhotoUrl { get; set; }
		public List<ProductCategoryDto> Categories { get; set; }
		public ProductVariantDto Variant { get; set; }
		public string ProductTypeName { get; set; }
		public string Tags { get; set; }

	}
}
