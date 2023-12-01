using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Models.Products;
using GloriaFestasCatalogo.Shared.Utils;
using System.Net.Http.Json;

namespace GloriaFestasCatalogo.Client.Services.ProductService
{
	public class ProductService : IProductService
	{

		private readonly HttpClient _http;

		public ProductService(HttpClient http)
		{
			_http = http;
		}

		public async Task<ServiceResponse<List<ProductDto>>> GetProductsAsync()
		{
			var result = await _http.GetFromJsonAsync<ServiceResponse<List<ProductDto>>>($"api/product/all");
			return result;
		}

		public async Task<ServiceResponse<ProductResponse>> GetProductsPageableAsync(int page, int pageSize, int categoryId, string? text = null)
		{
			var url = $"api/product/page/{page}?pageSize={pageSize}";

			url += $"&categoryId={categoryId}";

			if (!string.IsNullOrEmpty(text))
			{
				url += $"&text={Uri.EscapeDataString(text)}";
			}

			var result = await _http.GetFromJsonAsync<ServiceResponse<ProductResponse>>(url);
			return result;
		}

		public async Task<ServiceResponse<ProductDto>> GetProductAsync(int productId)
		{
			var result = await _http.GetFromJsonAsync<ServiceResponse<ProductDto>>($"api/product/{productId}");
			return result;
		}

		public async Task<ServiceResponse<List<ProductDto>>> GetProductsByCategory(int categoryId)
		{
			var result = await _http.GetFromJsonAsync<ServiceResponse<List<ProductDto>>>($"api/product/category/{categoryId}");
			return result;
		}

		public Task<ServiceResponse<ProductDto>> CreateProduct(Product product)
		{
			throw new NotImplementedException();
		}

		public Task<ServiceResponse<bool>> DeleteProduct(int productId)
		{
			throw new NotImplementedException();
		}

		public Task<ServiceResponse<ProductDto>> UpdateProduct(Product product)
		{
			throw new NotImplementedException();
		}
	}
}
