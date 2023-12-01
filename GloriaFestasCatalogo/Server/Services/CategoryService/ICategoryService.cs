using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Utils;

namespace GloriaFestasCatalogo.Server.Services.CategoryService
{
	public interface ICategoryService
	{

		Task<ServiceResponse<List<ProductCategoryDto>>> GetCategoriesAsync();

	}
}
