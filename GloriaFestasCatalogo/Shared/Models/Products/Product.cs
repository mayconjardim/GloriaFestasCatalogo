namespace GloriaFestasCatalogo.Shared.Models.Products
{
	public class Product
	{

		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string PhotoUrl { get; set; }
		public List<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
		public List<ProductCategory> Categories { get; set; } = new List<ProductCategory>();
		public string Tags { get; set; } = string.Empty;
		public bool Active { get; set; } = true;
		public bool Featured { get; set; } = false;

	}
}
