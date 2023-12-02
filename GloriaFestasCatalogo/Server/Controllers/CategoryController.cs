using GloriaFestasCatalogo.Server.Services.CategoryService;
using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GloriaFestasCatalogo.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{

		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		public async Task<ActionResult<ServiceResponse<List<ProductCategoryDto>>>> GetCategories()
		{
			var result = await _categoryService.GetCategoriesAsync();
			return Ok(result);
		}

		[HttpGet("{categoryId}")]
		public async Task<ActionResult<ServiceResponse<ProductCategoryDto>>> GetCategorie(int categoryId)
		{
			var result = await _categoryService.GetCategorieAsync(categoryId);
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<ServiceResponse<ProductCategoryDto>>> CreateProduct(ProductCategoryDto request)
		{
			return Ok(await _categoryService.CreateCategorie(request));
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<ServiceResponse<ProductCategoryDto>>> UpdateCategorie(ProductCategoryDto updatedCategory)
		{

			var response = await _categoryService.UpdateCategorie(updatedCategory);

			if (response.Success)
			{
				return Ok(response);
			}

			return BadRequest(response);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<ServiceResponse<bool>>> DeleteCategorie(int id)
		{
			var response = await _categoryService.DeleteCategorie(id);

			if (response.Success)
			{
				return Ok(response);
			}

			return NotFound(response);
		}

	}
}
