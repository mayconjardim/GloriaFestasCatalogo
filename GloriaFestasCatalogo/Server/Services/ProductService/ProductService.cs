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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<List<ProductDto>>> GetProductsByCategory(int categoryId)
        {

            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.Category.Id.Equals(categoryId) && p.Active)
                    .ToListAsync()
            };

            return response;
        }

    }
}
