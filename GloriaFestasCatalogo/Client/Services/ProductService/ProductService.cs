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

		public async Task<ServiceResponse<ProductDto>> CreateProduct(ProductCreateDto product)
		{
			var result = await _http.PostAsJsonAsync("api/product", product);

			var newProduct = await result.Content.ReadFromJsonAsync<ServiceResponse<ProductDto>>();

			return newProduct;
		}

		public async Task<ServiceResponse<bool>> DeleteProduct(int productId)
		{
			var response = new ServiceResponse<bool>();

			try
			{
				var result = await _http.DeleteAsync($"api/product/{productId}");
				response = await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
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
				var result = await _http.PutAsJsonAsync($"api/product/active/{activeOr.ProductId}", activeOr);
				response = await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
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
				var result = await _http.PutAsJsonAsync($"api/product/{updatedProduct.Id}", updatedProduct);
				response = await result.Content.ReadFromJsonAsync<ServiceResponse<ProductDto>>();
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
