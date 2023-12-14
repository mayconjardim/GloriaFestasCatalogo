using AutoMapper;
using GloriaFestasCatalogo.Server.Data;
using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Models.Products;
using GloriaFestasCatalogo.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace GloriaFestasCatalogo.Server.Services.ProductService
{
	public class ProductService : IProductService
	{

		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public ProductService(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;

		}

		public async Task<ServiceResponse<List<ProductDto>>> GetProductsAsync()
		{
			var products = await _context.Products
				.Include(p => p.Category)
				.Where(p => p.Active)
				.ToListAsync();

			var productDtos = _mapper.Map<List<ProductDto>>(products);

			var response = new ServiceResponse<List<ProductDto>>
			{
				Data = productDtos
			};

			return response;
		}

		public async Task<ServiceResponse<ProductResponse>> GetProductsPageableAsync(int page, int pageSize, int categoryId, string text = null)
		{

			IQueryable<Product> query = _context.Products.Include(p => p.Category);

			if (!string.IsNullOrEmpty(text))
			{
				query = query.Where(p => EF.Functions.Like(p.Name, $"%{text}%") || EF.Functions.Like(p.Tags, $"%{text}%"));
			}

			if (categoryId != 0)
			{
				query = query.Where(p => p.Category.Id == categoryId);
			}

			var totalProducts = await query.CountAsync();

			var products = await query
				.Skip((page - 1) * pageSize)
				.Include(p => p.Category)
				.Take(pageSize)
				.ToListAsync();

			var productDtos = _mapper.Map<List<ProductDto>>(products);

			if (productDtos == null || productDtos.Count == 0)
			{
				var emptyResponse = new ServiceResponse<ProductResponse>
				{
					Success = true,
					Data = new ProductResponse
					{
						Products = new List<ProductDto>(),
						Pages = 0,
						CurrentPage = page
					},
					Message = "Não foram encontrados produtos para os critérios fornecidos."
				};

				return emptyResponse;
			}

			var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

			var response = new ServiceResponse<ProductResponse>
			{
				Data = new ProductResponse
				{
					Products = productDtos,
					Pages = totalPages,
					CurrentPage = page
				}
			};

			return response;
		}

		public async Task<ServiceResponse<ProductDto>> GetProductAsync(int productId)
		{
			var response = new ServiceResponse<ProductDto>();

			var product = await _context.Products
				.Include(p => p.Category)
				.Where(p => p.Id == productId && p.Active)
				.FirstOrDefaultAsync();

			if (product == null)
			{
				response.Success = false;
				response.Message = "Desculpe, mas este produto não existe.";
			}
			else
			{
				var productDto = _mapper.Map<ProductDto>(product);
				response.Data = productDto;
			}

			return response;
		}

		public async Task<ServiceResponse<List<ProductDto>>> GetProductsByCategory(int categoryId)
		{
			var products = await _context.Products
				.Include(p => p.Category)
				.Where(p => p.Category.Id.Equals(categoryId) && p.Active)
				.ToListAsync();

			var productDtos = _mapper.Map<List<ProductDto>>(products);

			var response = new ServiceResponse<List<ProductDto>>
			{
				Data = productDtos
			};

			return response;
		}

		public async Task<ServiceResponse<ProductDto>> CreateProduct(ProductCreateDto product)
		{

			var response = new ServiceResponse<ProductDto>();


			var category = await _context.Categories.Where(c => c.Id == product.ProductCategoryId).FirstOrDefaultAsync();

			var newProduct = _mapper.Map<Product>(product);

			if (category != null)
			{
				newProduct.Category = category;
			}

			try
			{
				_context.Add(newProduct);
				await _context.SaveChangesAsync();
				response.Success = false;
				response.Data = _mapper.Map<ProductDto>(newProduct);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public async Task<ServiceResponse<ProductDto>> UpdateProduct(ProductDto updatedProduct)
		{
			var response = new ServiceResponse<ProductDto>();

			try
			{
				var category = await _context.Categories.Where(c => c.Id == updatedProduct.Category.Id).FirstOrDefaultAsync();

				var product = await _context.Products.Where(p => p.Id == updatedProduct.Id).FirstOrDefaultAsync();

				if (product != null && category != null)
				{
					product.Name = updatedProduct.Name;
					product.Price = updatedProduct.Price;
					product.PhotoUrl = updatedProduct.PhotoUrl;
					product.Tags = updatedProduct.Tags;
					product.Category = category;

					_context.Update(product);
					await _context.SaveChangesAsync();

					response.Success = true;
					response.Data = _mapper.Map<ProductDto>(product);
				}
				else
				{
					response.Success = false;
					response.Message = "Produto ou categoria não encontrados.";
				}
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}


		public async Task<ServiceResponse<bool>> DeleteProduct(int productId)
		{
			var response = new ServiceResponse<bool>();

			try
			{
				var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

				if (product == null)
				{
					response.Success = false;
					response.Message = "Produto não encontrado.";
					return response;
				}

				_context.Products.Remove(product);
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

		public async Task<ServiceResponse<bool>> ActiveOrDeactiveProduct(ActiveOrDeactive activeOr)
		{
			var response = new ServiceResponse<bool>();

			try
			{
				var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == activeOr.ProductId);

				if (product == null)
				{
					response.Success = false;
					response.Message = "Produto não encontrado.";
					return response;
				}
				await Console.Out.WriteLineAsync(" O RESULTADO É = " + activeOr.Active);


				product.Active = activeOr.Active;
				_context.Update(product);

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
