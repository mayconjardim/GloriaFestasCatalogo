﻿using AutoMapper;
using GloriaFestasCatalogo.Server.Data;
using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Models.Products;
using GloriaFestasCatalogo.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace GloriaFestasCatalogo.Server.Services.CategoryService
{
	public class CategoryService : ICategoryService
	{

		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public CategoryService(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<ServiceResponse<List<ProductCategoryDto>>> GetCategoriesAsync(string? text = null)
		{
			var response = new ServiceResponse<List<ProductCategoryDto>>();

			IQueryable<ProductCategory> query = _context.Categories;

			if (!string.IsNullOrEmpty(text))
			{
				query = query.Where(p => EF.Functions.Like(p.Name, $"%{text}%"));
			}

			var category = await query.ToListAsync();

			var categoryDto = _mapper.Map<List<ProductCategoryDto>>(category);
			response.Success = true;
			response.Data = categoryDto;

			return response;
		}

		public async Task<ServiceResponse<List<ProductCategoryDto>>> GetCategoriesOrderActivesAsync()
		{
			var response = new ServiceResponse<List<ProductCategoryDto>>();

			 var categories = await _context.Categories.Where(c => c.Active == true)
				.OrderBy(c => c.Order).ToListAsync();

			var categoryDto = _mapper.Map<List<ProductCategoryDto>>(categories);
			response.Success = true;
			response.Data = categoryDto;

			return response;
		}

		public async Task<ServiceResponse<ProductCategoryDto>> GetCategorieAsync(int id)
		{
			var response = new ServiceResponse<ProductCategoryDto>();

			var category = await _context.Categories
				.Where(p => p.Id == id)
				.FirstOrDefaultAsync();

			if (category == null)
			{
				response.Success = false;
				response.Message = "Desculpe, mas este categoria não existe.";
			}
			else
			{
				var categoryDto = _mapper.Map<ProductCategoryDto>(category);
				response.Data = categoryDto;
			}

			return response;
		}

		public async Task<ServiceResponse<ProductCategoryDto>> CreateCategorie(ProductCategoryDto categoryDto)
		{
			var response = new ServiceResponse<ProductCategoryDto>();

			var newCategory = _mapper.Map<ProductCategory>(categoryDto);

			try
			{
				_context.Add(newCategory);
				await _context.SaveChangesAsync();
				response.Success = true;
				response.Data = _mapper.Map<ProductCategoryDto>(newCategory);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public async Task<ServiceResponse<ProductCategoryDto>> UpdateCategorie(ProductCategoryDto categoryDto)
		{
			var response = new ServiceResponse<ProductCategoryDto>();

			try
			{

				var category = await _context.Categories.Where(p => p.Id == categoryDto.Id).FirstOrDefaultAsync();

				if (category != null)
				{
					category.Name = categoryDto.Name;
					_context.Update(category);
					await _context.SaveChangesAsync();

					response.Success = true;
					response.Data = _mapper.Map<ProductCategoryDto>(category);
				}
				else
				{
					response.Success = false;
					response.Message = "Categoria não encontrados.";
				}
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public async Task<ServiceResponse<bool>> DeleteCategorie(int id)
		{
			var response = new ServiceResponse<bool>();

			try
			{
				var category = await _context.Categories
					.Include(c => c.Products) 
					.FirstOrDefaultAsync(c => c.Id == id);

				if (category == null)
				{
					response.Success = false;
					response.Message = "Categoria não encontrado.";
					return response;
				}
				
				if (category.Products.Any())
				{
					response.Success = false;
					response.Message = "Não é possível excluir a categoria, pois existem produtos associados a ela.";
					return response;
				}

				_context.Categories.Remove(category);
				await _context.SaveChangesAsync();

				response.Success = true;
				response.Data = true;
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public async Task<ServiceResponse<bool>> UpdateCategoryOrderAsync(int categoryId, CategoryOrder order)
		{
			
			var response = new ServiceResponse<bool>();

			try
			{
				var category = await _context.Categories.FindAsync(categoryId);

				if (category != null)
				{
					category.Order = order.NewOrder;
					await _context.SaveChangesAsync();
					
					response.Success = true;
					response.Data = true;
				}
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}
			return response;
		}
		
		
		public async Task<ServiceResponse<bool>> ActiveOrDeactiveCategory(int id, ActiveOrDeactive activeOr)
		{
			try
			{
				var category= await _context.Categories.FindAsync(id);

				if (category == null)
				{
					return new ServiceResponse<bool>
					{
						Success = false,
						Message = "Categoria não encontrada."
					};
				}

				category.Active = !category.Active;

				_context.Update(category);
				await _context.SaveChangesAsync();

				return new ServiceResponse<bool> { Success = true, Data = true };

			}
			catch (Exception ex)
			{
				return new ServiceResponse<bool>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}
		
	}
}
