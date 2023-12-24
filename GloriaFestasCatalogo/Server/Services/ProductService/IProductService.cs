using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Utils;

namespace GloriaFestasCatalogo.Server.Services.ProductService
{
	public interface IProductService
	{

		Task<ServiceResponse<List<ProductDto>>> GetProductsAsync();
		Task<ServiceResponse<ProductResponse>> GetProductsPageableAsync(int page, int pageSize, int categoryId, string text = null);
		Task<ServiceResponse<ProductResponse>> GetActiveProductsPageableAsync(int page, int pageSize, int categoryId, string text = null);
		Task<ServiceResponse<ProductDto>> GetProductAsync(int productId);
		Task<ServiceResponse<List<ProductDto>>> GetProductsByCategory(int categoryId);
		Task<ServiceResponse<ProductDto>> CreateProduct(ProductCreateDto product);
		Task<ServiceResponse<ProductDto>> UpdateProduct(ProductDto product);
		Task<ServiceResponse<bool>> DeleteProduct(int productId);
		Task<ServiceResponse<bool>> ActiveOrDeactiveProduct(int id, ActiveOrDeactive activeOr);

	}
}
