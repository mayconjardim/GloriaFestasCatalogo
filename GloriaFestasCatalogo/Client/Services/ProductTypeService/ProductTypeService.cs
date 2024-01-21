using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Utils;
using System.Net.Http.Json;

namespace GloriaFestasCatalogo.Client.Services.CategoryService
{
	public class ProductTypeService : IProductTypeService
	{

		private readonly HttpClient _http;

		public ProductTypeService(HttpClient http)
		{
			_http = http;
		}

		public async Task<ServiceResponse<List<ProductTypeDto>>> GetProductTypeAsync(string? text = null)
		{
			var url = "api/producttype/";

			if (!string.IsNullOrEmpty(text))
			{
				url += (url.Contains("?") ? "&" : "?") + $"text={Uri.EscapeDataString(text)}";
			}

			var result = await _http.GetFromJsonAsync<ServiceResponse<List<ProductTypeDto>>>(url);
			return result;
		}

		public async Task<ServiceResponse<ProductTypeDto>> GetProductTypeAsync(int id)
		{
			var result = await _http.GetFromJsonAsync<ServiceResponse<ProductTypeDto>>($"api/producttype/{id}");
			return result;
		}

		public async Task<ServiceResponse<ProductTypeDto>> CreateProductType(ProductTypeDto productTypeDto)
		{
			var result = await _http.PostAsJsonAsync("api/producttype", productTypeDto);

			var newType = await result.Content.ReadFromJsonAsync<ServiceResponse<ProductTypeDto>>();

			return newType;
		}

		public async Task<ServiceResponse<ProductTypeDto>> UpdateProductType(ProductTypeDto productTypeDto)
		{
			var response = new ServiceResponse<ProductTypeDto>();

			try
			{
				var result = await _http.PutAsJsonAsync($"api/producttype/{productTypeDto.Id}", productTypeDto);
				response = await result.Content.ReadFromJsonAsync<ServiceResponse<ProductTypeDto>>();
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public async Task<ServiceResponse<bool>> DeleteProductType(int typeId)
		{
			var response = new ServiceResponse<bool>();

			try
			{
				var result = await _http.DeleteAsync($"api/producttype/{typeId}");
				response = await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
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
