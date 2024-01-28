using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Utils;

namespace GloriaFestasCatalogo.Client.Services.CategoryService
{
	public interface ICategoryService
	{

		Task<ServiceResponse<List<ProductCategoryDto>>> GetCategoriesAsync(string? text = null);
		Task<ServiceResponse<ProductCategoryDto>> GetCategorieAsync(int id);
		Task<ServiceResponse<ProductCategoryDto>> CreateCategorie(ProductCategoryDto categoryDto);
		Task<ServiceResponse<ProductCategoryDto>> UpdateCategorie(ProductCategoryDto categoryDto);
		Task<ServiceResponse<bool>> DeleteCategorie(int id);
		Task<ServiceResponse<bool>> UpdateCategoryOrderAsync(int categoryId, CategoryOrder order);
		Task<ServiceResponse<bool>> ActiveOrDeactiveCategory(ActiveOrDeactive activeOr);

	}
}
