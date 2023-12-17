using GloriaFestasCatalogo.Shared.Dtos.Products;
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
			return await _http.GetFromJsonAsync<ServiceResponse<List<ProductDto>>>("api/product/all");
		}

		public async Task<ServiceResponse<ProductResponse>> GetProductsPageableAsync(int page, int pageSize, int categoryId, string? text = null)
		{
			var url = $"api/product/page/{page}?pageSize={pageSize}&categoryId={categoryId}";

			if (!string.IsNullOrEmpty(text))
			{
				url += $"&text={Uri.EscapeDataString(text)}";
			}

			return await _http.GetFromJsonAsync<ServiceResponse<ProductResponse>>(url);
		}

		public async Task<ServiceResponse<ProductDto>> GetProductAsync(int productId)
		{
			return await _http.GetFromJsonAsync<ServiceResponse<ProductDto>>($"api/product/{productId}");
		}

		public async Task<ServiceResponse<List<ProductDto>>> GetProductsByCategory(int categoryId)
		{
			return await _http.GetFromJsonAsync<ServiceResponse<List<ProductDto>>>($"api/product/category/{categoryId}");
		}

		public async Task<ServiceResponse<ProductDto>> CreateProduct(ProductCreateDto product)
		{
			var result = await _http.PostAsJsonAsync("api/product", product);
			return await result.Content.ReadFromJsonAsync<ServiceResponse<ProductDto>>();
		}

		public async Task<ServiceResponse<bool>> DeleteProduct(int productId)
		{
			try
			{
				var result = await _http.DeleteAsync($"api/product/{productId}");
				return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
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

		public async Task<ServiceResponse<bool>> ActiveOrDeactiveProduct(ActiveOrDeactive activeOr)
		{
			try
			{
				var result = await _http.PutAsJsonAsync($"api/product/active/{activeOr.ProductId}", activeOr);
				return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
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

		public async Task<ServiceResponse<ProductDto>> UpdateProduct(ProductDto updatedProduct)
		{
			try
			{
				var result = await _http.PutAsJsonAsync($"api/product/{updatedProduct.Id}", updatedProduct);
				return await result.Content.ReadFromJsonAsync<ServiceResponse<ProductDto>>();
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

	}
}
