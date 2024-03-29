﻿using GloriaFestasCatalogo.Server.Services.CategoryService;
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
		public async Task<ActionResult<ServiceResponse<List<ProductCategoryDto>>>> GetCategories(string? text = null)
		{
			var result = await _categoryService.GetCategoriesAsync(text);
			return Ok(result);
		}
		
		[HttpGet("active")]
		public async Task<ActionResult<ServiceResponse<List<ProductCategoryDto>>>> GetCategoriesOrderActivesAsync()
		{
			var result = await _categoryService.GetCategoriesOrderActivesAsync();
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
		
		[HttpPut("order/{categoryId}")]
		public async Task<ActionResult> UpdateCategoryOrderAsync(int categoryId, CategoryOrder order)
		{
			var response = await _categoryService.UpdateCategoryOrderAsync(categoryId, order);

			if (!response.Success)
			{
				return BadRequest(response);
			}

			return Ok(response);
		}

		[HttpPut("active/{id}")]
		public async Task<ActionResult<ServiceResponse<bool>>> ActiveOrDeactiveCategory(int id, ActiveOrDeactive activeOr)
		{
			try
			{
				var response = await _categoryService.ActiveOrDeactiveCategory(id, activeOr);

				if (response.Success)
				{
					return Ok(response);
				}

				return BadRequest(response);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new ServiceResponse<bool>
				{
					Success = false,
					Message = ex.Message
				});
			}
		}
		
	}
}
