using AutoMapper;
using GloriaFestasCatalogo.Server.Data;
using GloriaFestasCatalogo.Shared.Dtos.Products;
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

		public async Task<ServiceResponse<List<ProductCategoryDto>>> GetCategoriesAsync()
		{
			var response = new ServiceResponse<List<ProductCategoryDto>>();

			var category = await _context.Categories.ToListAsync();

			var categoryDto = _mapper.Map<List<ProductCategoryDto>>(category);
			response.Success = true;
			response.Data = categoryDto;

			return response;
		}
	}
}
