using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Utils;

namespace GloriaFestasCatalogo.Server.Services.CategoryService
{
	public interface ICategoryService
	{

		Task<ServiceResponse<List<ProductCategoryDto>>> GetCategoriesAsync();
		Task<ServiceResponse<ProductCategoryDto>> GetCategorieAsync(int id);
		Task<ServiceResponse<ProductCategoryDto>> CreateCategorie(ProductCategoryDto categoryDto);
		Task<ServiceResponse<ProductCategoryDto>> UpdateCategorie(ProductCategoryDto categoryDto);
		Task<ServiceResponse<bool>> DeleteCategorie(int id);

	}
}
