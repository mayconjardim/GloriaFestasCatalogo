using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Models.Products;
using GloriaFestasCatalogo.Shared.Utils;

namespace GloriaFestasCatalogo.Client.Services.ProductService
{
	public interface IProductService
	{

		Task<ServiceResponse<List<ProductDto>>> GetProductsAsync();
		Task<ServiceResponse<ProductResponse>> GetProductsPageableAsync(int page, int pageSize, int categoryId, string text = null);
		Task<ServiceResponse<ProductDto>> GetProductAsync(int productId);
		Task<ServiceResponse<List<ProductDto>>> GetProductsByCategory(int categoryId);
		Task<ServiceResponse<ProductDto>> CreateProduct(ProductCreateDto product);
		Task<ServiceResponse<ProductDto>> UpdateProduct(Product product);
		Task<ServiceResponse<bool>> DeleteProduct(int productId);

	}
}
