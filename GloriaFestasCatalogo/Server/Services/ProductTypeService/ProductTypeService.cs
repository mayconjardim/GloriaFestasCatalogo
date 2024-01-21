using AutoMapper;
using GloriaFestasCatalogo.Server.Data;
using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Models.Products;
using GloriaFestasCatalogo.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace GloriaFestasCatalogo.Server.Services.ProductTypeService
{
	public class ProductTypeService : IProductTypeService
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public ProductTypeService(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<ServiceResponse<List<ProductTypeDto>>> GetProductTypes(string? text = null)
		{
			var response = new ServiceResponse<List<ProductTypeDto>>();

			IQueryable<ProductType> query = _context.ProductTypes;

			if (!string.IsNullOrEmpty(text))
			{
				query = query.Where(p => EF.Functions.Like(p.Name, $"%{text}%"));
			}

			var types = await query.ToListAsync();
			var typeDto = _mapper.Map<List<ProductTypeDto>>(types);
			response.Success = true;
			response.Data = typeDto;

			return response;
		}

		public async Task<ServiceResponse<ProductTypeDto>> CreateProductType(ProductTypeDto productTypeDto)
		{
			var response = new ServiceResponse<ProductTypeDto>();

			var newType = _mapper.Map<ProductType>(productTypeDto);

			try
			{
				_context.Add(newType);
				await _context.SaveChangesAsync();
				response.Success = true;
				response.Data = _mapper.Map<ProductTypeDto>(newType);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public async Task<ServiceResponse<ProductTypeDto>> UpdateProductType(ProductTypeDto productTypeDto)
		{
			var response = new ServiceResponse<ProductTypeDto>();

			try
			{

				var type = await _context.ProductTypes.Where(p => p.Id == productTypeDto.Id).FirstOrDefaultAsync();

				if (type != null)
				{
					type.Name = productTypeDto.Name;
					_context.Update(type);
					await _context.SaveChangesAsync();

					response.Success = true;
					response.Data = _mapper.Map<ProductTypeDto>(type);
				}
				else
				{
					response.Success = false;
					response.Message = "Tipo de Produto não encontrados.";
				}
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public async Task<ServiceResponse<bool>> DeleteProductType(int id)
		{
			var response = new ServiceResponse<bool>();

			try
			{
				var type = await _context.ProductTypes.FindAsync(id);

				if (type == null)
				{
					response.Success = false;
					response.Message = "Tipo de Produto não encontrado.";
					return response;
				}

				_context.ProductTypes.Remove(type);
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
	}
}
