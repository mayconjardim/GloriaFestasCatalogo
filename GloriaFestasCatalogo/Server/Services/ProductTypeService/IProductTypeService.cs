using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Utils;

namespace GloriaFestasCatalogo.Server.Services.ProductTypeService
{
	public interface IProductTypeService
	{

		Task<ServiceResponse<List<ProductTypeDto>>> GetProductTypes(string? text = null);
		Task<ServiceResponse<ProductTypeDto>> GetProductTypeAsync(int id);
		Task<ServiceResponse<ProductTypeDto>> CreateProductType(ProductTypeDto productType);
		Task<ServiceResponse<ProductTypeDto>> UpdateProductType(ProductTypeDto productType);
		Task<ServiceResponse<bool>> DeleteProductType(int id);

	}
}
