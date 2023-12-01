using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Utils;
using System.Net.Http.Json;

namespace GloriaFestasCatalogo.Client.Services.CategoryService
{
	public class CategoryService : ICategoryService
	{

		private readonly HttpClient _http;

		public CategoryService(HttpClient http)
		{
			_http = http;
		}

		public async Task<ServiceResponse<List<ProductCategoryDto>>> GetCategoriesAsync()
		{
			var result = await _http.GetFromJsonAsync<ServiceResponse<List<ProductCategoryDto>>>($"api/category");
			return result;
		}

	}
}
