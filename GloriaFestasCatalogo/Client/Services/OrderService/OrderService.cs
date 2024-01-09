using GloriaFestasCatalogo.Shared.Dtos.Orders;
using GloriaFestasCatalogo.Shared.Utils;
using System.Net;
using System.Net.Http.Json;

namespace GloriaFestasCatalogo.Client.Services.OrderService
{
	public class OrderService : IOrderService
	{

		private readonly HttpClient _http;

		public OrderService(HttpClient http)
		{
			_http = http;
		}

		public async Task<ServiceResponse<OrderDto>> CreateOrder(OrderCreateDto order)
		{
			var result = await _http.PostAsJsonAsync("api/order", order);

			var newOrder = await result.Content.ReadFromJsonAsync<ServiceResponse<OrderDto>>();

			return newOrder;

		}

		public async Task<ServiceResponse<bool>> DeleteOrder(int orderId)
		{
			var response = new ServiceResponse<bool>();

			try
			{
				var result = await _http.DeleteAsync($"api/order/{orderId}");
				response = await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public async Task<ServiceResponse<OrderDto>> GetOrderById(int orderId)
		{
			var result = await _http.GetFromJsonAsync<ServiceResponse<OrderDto>>($"api/order/{orderId}");
			return result;
		}

		public async Task<ServiceResponse<OrderResponse>> GetOrderPageableAsync(int page, int pageSize, string text = null, OrderStatus? status = null)
		{
			var url = $"api/order/page/{page}?pageSize={pageSize}";

			if (!string.IsNullOrEmpty(text))
			{
				url += $"&text={Uri.EscapeDataString(text)}";
			}

			if (status.HasValue)
			{
				url += $"&status={Uri.EscapeDataString(status.ToString())}";
			}

			try
			{
				var result = await _http.GetFromJsonAsync<ServiceResponse<OrderResponse>>(url);

				return result;
			}
			catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
			{
				var notFoundResponse = new ServiceResponse<OrderResponse>
				{
					Success = false,
					Message = "O recurso não foi encontrado.",
				};

				return notFoundResponse;
			}
			catch (Exception ex)
			{
				var errorResponse = new ServiceResponse<OrderResponse>
				{
					Success = false,
					Message = "Ocorreu um erro ao processar a solicitação.",
				};

				return errorResponse;
			}
		}

		public async Task<ServiceResponse<OrderDto>> UpdateOrder(OrderDto order)
		{
			var result = await _http.PutAsJsonAsync($"api/order", order);
			var content = await result.Content.ReadFromJsonAsync<ServiceResponse<OrderDto>>();
			return content;
		}

	}
}
