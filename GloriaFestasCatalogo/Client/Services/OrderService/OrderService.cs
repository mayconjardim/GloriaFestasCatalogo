using GloriaFestasCatalogo.Shared.Dtos.Orders;
using GloriaFestasCatalogo.Shared.Utils;
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

		public async Task<ServiceResponse<OrderDto>> GetOrderById(int orderId)
		{
			var result = await _http.GetFromJsonAsync<ServiceResponse<OrderDto>>($"api/order/{orderId}");
			return result;
		}

		public async Task<ServiceResponse<OrderResponse>> GetOrderPageableAsync(int page, int pageSize, string text = null, OrderStatus? status = null)
		{
			// Construa a URL adicionando parâmetros opcionais
			var url = $"api/order/page/{page}?pageSize={pageSize}";

			if (!string.IsNullOrEmpty(text))
			{
				url += $"&text={Uri.EscapeDataString(text)}";
			}

			if (status.HasValue)
			{
				url += $"&status={Uri.EscapeDataString(status.ToString())}";
			}

			// Faça a chamada HTTP com a URL construída
			var result = await _http.GetFromJsonAsync<ServiceResponse<OrderResponse>>(url);

			return result;
		}


		public async Task<ServiceResponse<OrderDto>> UpdateOrder(OrderDto order)
		{
			var result = await _http.PutAsJsonAsync($"api/order", order);
			var content = await result.Content.ReadFromJsonAsync<ServiceResponse<OrderDto>>();
			return content;
		}

	}
}
