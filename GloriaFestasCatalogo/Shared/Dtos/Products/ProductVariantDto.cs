namespace GloriaFestasCatalogo.Shared.Dtos.Products
{
	public class ProductVariantDto
	{

		public int ProductId { get; set; }
		public int ProductTypeId { get; set; }
		public string? ProductTypeName { get; set; }
		public decimal Price { get; set; } = 0.00m;

	}
}
