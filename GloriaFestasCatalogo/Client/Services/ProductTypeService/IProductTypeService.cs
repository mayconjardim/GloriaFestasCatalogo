using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Utils;

namespace GloriaFestasCatalogo.Client.Services.CategoryService
{
	public interface IProductTypeService
	{

		Task<ServiceResponse<List<ProductTypeDto>>> GetProductTypeAsync(string? text = null);
		Task<ServiceResponse<ProductTypeDto>> GetProductTypeAsync(int id);
		Task<ServiceResponse<ProductTypeDto>> CreateProductType(ProductTypeDto productTypeDto);
		Task<ServiceResponse<ProductTypeDto>> UpdateProductType(ProductTypeDto productTypeDto);
		Task<ServiceResponse<bool>> DeleteProductType(int id);

	}
}
