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

        public async Task<ServiceResponse<ProductResponse>> GetProductsPageableAsync(int page, int pageSize = 10)
        {
            var totalProducts = await _context.Products
                .CountAsync(p => p.Active);

            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Active)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var productDtos = _mapper.Map<List<ProductDto>>(products);

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

        public Task<ServiceResponse<ProductDto>> CreateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<ProductDto>> UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> DeleteProduct(int productId)
        {
            throw new NotImplementedException();
        }

    }
}
