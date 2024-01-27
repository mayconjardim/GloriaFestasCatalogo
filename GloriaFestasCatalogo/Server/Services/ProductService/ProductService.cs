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

        private async Task<bool> ProductExistsAsync(int productId)
        {
            return await _context.Products.AnyAsync(p => p.Id == productId);
        }

        public async Task<ServiceResponse<List<ProductDto>>> GetProductsAsync()
        {
            var products = await _context.Products
                .Include(p => p.Categories)
                .Include(p => p.Variants)
                .Where(p => p.Active)
                .ToListAsync();

            var productDtos = _mapper.Map<List<ProductDto>>(products);

            return new ServiceResponse<List<ProductDto>> { Data = productDtos };
        }

        public async Task<ServiceResponse<ProductResponse>> GetProductsPageableAsync(int page, int pageSize, int categoryId, string text = null)
        {

            IQueryable<Product> query = _context.Products
                .Include(p => p.Categories)
                .Include(p => p.Variants)
                .ThenInclude(v => v.ProductType);

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
                .Include(p => p.Variants)
                .ThenInclude(v => v.ProductType)
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
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .ThenInclude(v => v.ProductType)
                .Where(p => p.Id == productId && p.Active)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return new ServiceResponse<ProductDto>
                {
                    Success = false,
                    Message = "Desculpe, mas este produto não existe."
                };
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return new ServiceResponse<ProductDto> { Data = productDto };
        }

        public async Task<ServiceResponse<List<ProductDto>>> GetProductsByCategory(int categoryId)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .ThenInclude(v => v.ProductType)
                .Where(p => p.Category.Id.Equals(categoryId) && p.Active)
                .ToListAsync();

            var productDtos = _mapper.Map<List<ProductDto>>(products);

            return new ServiceResponse<List<ProductDto>> { Data = productDtos };
        }

        public async Task<ServiceResponse<ProductDto>> CreateProduct(ProductCreateDto product)
        {
            var response = new ServiceResponse<ProductDto>();

            try
            {
                var category = await _context.Categories.FindAsync(product.ProductCategoryId);

                if (category == null)
                {
                    response.Success = false;
                    response.Message = "Categoria não encontrada.";
                    return response;
                }

                var existingProduct = await _context.Products
                    .Where(p => p.Name == product.Name && p.Category.Id == category.Id)
                    .FirstOrDefaultAsync();

                if (existingProduct != null)
                {
                    response.Success = false;
                    response.Message = "Já existe um produto com o mesmo nome nesta categoria.";
                    return response;
                }

                var newProduct = _mapper.Map<Product>(product);
                newProduct.Category = category;
                _context.Add(newProduct);
                await _context.SaveChangesAsync();


                foreach (var variant in product.Variants)
                {
                    newProduct.Variants.Add(new ProductVariant { Price = variant.Price, ProductId = newProduct.Id, ProductTypeId = variant.ProductTypeId });
                }

                _context.Update(newProduct);
                await _context.SaveChangesAsync();


                response.Success = true;
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
            try
            {
                var category = await _context.Categories.FindAsync(updatedProduct.Category.Id);

                if (category == null)
                {
                    return new ServiceResponse<ProductDto>
                    {
                        Success = false,
                        Message = "Categoria não encontrada."
                    };
                }

                var product = await _context.Products
                    .Include(p => p.Variants)  // Inclua as variantes para evitar uma consulta adicional
                    .FirstOrDefaultAsync(p => p.Id == updatedProduct.Id);

                if (product == null)
                {
                    return new ServiceResponse<ProductDto>
                    {
                        Success = false,
                        Message = "Produto não encontrado."
                    };
                }

                product.Name = updatedProduct.Name;
                product.PhotoUrl = updatedProduct.PhotoUrl;
                product.Tags = updatedProduct.Tags;
                product.Description = updatedProduct.Description;
                product.Category = category;

                _context.ProductVariants.RemoveRange(product.Variants);

                foreach (var variant in updatedProduct.Variants)
                {
                    product.Variants.Add(new ProductVariant { Price = variant.Price, ProductTypeId = variant.ProductTypeId });
                }

                // Salvar as alterações no banco de dados
                await _context.SaveChangesAsync();

                return new ServiceResponse<ProductDto>
                {
                    Success = true,
                    Data = _mapper.Map<ProductDto>(product)
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ProductDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<bool>> DeleteProduct(int productId)
        {
            try
            {
                var productExists = await ProductExistsAsync(productId);

                if (!productExists)
                {
                    return new ServiceResponse<bool>
                    {
                        Success = false,
                        Message = "Produto não encontrado."
                    };
                }

                var product = await _context.Products.FindAsync(productId);
                _context.Products.Remove(product);
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

        public async Task<ServiceResponse<bool>> ActiveOrDeactiveProduct(int id, ActiveOrDeactive activeOr)
        {
            try
            {
                var productExists = await ProductExistsAsync(id);

                if (!productExists)
                {
                    return new ServiceResponse<bool>
                    {
                        Success = false,
                        Message = "Produto não encontrado."
                    };
                }

                var product = await _context.Products.FindAsync(id);
                product.Active = !product.Active;


                _context.Update(product);
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

        public async Task<ServiceResponse<ProductResponse>> GetActiveProductsPageableAsync(int page, int pageSize, int categoryId, string text = null)
        {
            IQueryable<Product> query = _context.Products.
                 Include(p => p.Category)
                .Include(p => p.Variants)
                .ThenInclude(v => v.ProductType);

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
                .Include(p => p.Variants)
                .ThenInclude(v => v.ProductType)
                .Where(p => p.Active == true)
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
    }
}
