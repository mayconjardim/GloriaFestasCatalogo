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
		public async Task<ActionResult<ServiceResponse<List<ProductDto>>>> GetProducts()
		{
			var result = await _categoryService.GetCategoriesAsync();
			return Ok(result);
		}

	}
}
