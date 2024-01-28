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

        public async Task<ServiceResponse<List<ProductCategoryDto>>> GetCategoriesAsync(string? text = null)
        {
            var url = "api/category/";

            if (!string.IsNullOrEmpty(text))
            {
                url += (url.Contains("?") ? "&" : "?") + $"text={Uri.EscapeDataString(text)}";
            }

            var result = await _http.GetFromJsonAsync<ServiceResponse<List<ProductCategoryDto>>>(url);
            return result;
        }

        public async Task<ServiceResponse<ProductCategoryDto>> GetCategorieAsync(int categoryId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<ProductCategoryDto>>($"api/category/{categoryId}");
            return result;
        }


        public async Task<ServiceResponse<ProductCategoryDto>> CreateCategorie(ProductCategoryDto categoryDto)
        {
            var result = await _http.PostAsJsonAsync("api/category", categoryDto);

            var newCategory = await result.Content.ReadFromJsonAsync<ServiceResponse<ProductCategoryDto>>();

            return newCategory;
        }

        public async Task<ServiceResponse<ProductCategoryDto>> UpdateCategorie(ProductCategoryDto categoryDto)
        {
            var response = new ServiceResponse<ProductCategoryDto>();

            try
            {
                var result = await _http.PutAsJsonAsync($"api/category/{categoryDto.Id}", categoryDto);
                response = await result.Content.ReadFromJsonAsync<ServiceResponse<ProductCategoryDto>>();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteCategorie(int categoryId)
        {
            var response = new ServiceResponse<bool>();

            try
            {
                var result = await _http.DeleteAsync($"api/category/{categoryId}");
                response = await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> UpdateCategoryOrderAsync(int categoryId, CategoryOrder order)
        {
            try
            {
                var result = await _http.PutAsJsonAsync($"api/category/order/{order.CategoryId}", order);
                return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
            }catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}