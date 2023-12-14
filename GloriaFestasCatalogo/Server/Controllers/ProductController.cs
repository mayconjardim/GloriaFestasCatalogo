using GloriaFestasCatalogo.Server.Services.ProductService;
using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Utils;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GloriaFestasCatalogo.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{

		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}

		[HttpGet("all")]
		public async Task<ActionResult<ServiceResponse<List<ProductDto>>>> GetProducts()
		{
			var result = await _productService.GetProductsAsync();
			return Ok(result);
		}

		[HttpGet("page/{page}")]
		public async Task<ActionResult<ServiceResponse<ProductResponse>>> GetProductsPageable(int page, int pageSize, int categoryId, string? text = null)
		{

			try
			{
				var result = await _productService.GetProductsPageableAsync(page, pageSize, categoryId, text);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Erro interno no servidor.");
			}

		}

		[HttpGet("{productId}")]
		public async Task<ActionResult<ServiceResponse<ProductDto>>> GetProduct(int productId)
		{
			var result = await _productService.GetProductAsync(productId);
			return Ok(result);
		}

		[HttpGet("category/{categoryId}")]
		public async Task<ActionResult<ServiceResponse<List<ProductDto>>>> GetProductsByCategory(int categoryId)
		{
			var result = await _productService.GetProductsByCategory(categoryId);
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<ServiceResponse<ProductDto>>> CreateProduct(ProductCreateDto request)
		{
			return Ok(await _productService.CreateProduct(request));
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<ServiceResponse<ProductDto>>> UpdateProduct(ProductDto updatedProduct)
		{

			var response = await _productService.UpdateProduct(updatedProduct);

			if (response.Success)
			{
				return Ok(response);
			}

			return BadRequest(response);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<ServiceResponse<bool>>> DeleteProduct(int id)
		{
			var response = await _productService.DeleteProduct(id);

			if (response.Success)
			{
				return Ok(response);
			}

			return NotFound(response);
		}


		[HttpPut("active/{id}")]
		public async Task<ActionResult<ServiceResponse<bool>>> ActiveOrDeactiveProduct(ActiveOrDeactive activeOr)
		{
			var response = await _productService.ActiveOrDeactiveProduct(activeOr);

			if (response.Success)
			{
				return Ok(response);
			}

			return NotFound(response);
		}


	}
}
