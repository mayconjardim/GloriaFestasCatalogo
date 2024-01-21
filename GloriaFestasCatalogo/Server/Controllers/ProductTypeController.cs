using GloriaFestasCatalogo.Server.Services.ProductTypeService;
using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GloriaFestasCatalogo.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductTypeController : ControllerBase
	{

		private readonly IProductTypeService _productTypeService;

		public ProductTypeController(IProductTypeService productTypeService)
		{
			_productTypeService = productTypeService;
		}

		[HttpGet]
		public async Task<ActionResult<ServiceResponse<List<ProductTypeDto>>>> GetProductTypes(string? text = null)
		{
			var result = await _productTypeService.GetProductTypes(text);
			return Ok(result);
		}

		[HttpGet("{typeId}")]
		public async Task<ActionResult<ServiceResponse<ProductTypeDto>>> GetProductTypeAsync(int typeId)
		{
			var result = await _productTypeService.GetProductTypeAsync(typeId);
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<ServiceResponse<ProductTypeDto>>> CreateProduct(ProductTypeDto request)
		{
			var response = await _productTypeService.CreateProductType(request);

			if (response.Success)
			{
				return Ok(response);
			}

			return BadRequest(response);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<ServiceResponse<ProductCategoryDto>>> UpdateProductType(ProductTypeDto updatedType)
		{

			var response = await _productTypeService.UpdateProductType(updatedType);

			if (response.Success)
			{
				return Ok(response);
			}

			return BadRequest(response);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<ServiceResponse<bool>>> DeleteProductType(int id)
		{
			var response = await _productTypeService.DeleteProductType(id);

			if (response.Success)
			{
				return Ok(response);
			}

			return NotFound(response);
		}

	}
}
