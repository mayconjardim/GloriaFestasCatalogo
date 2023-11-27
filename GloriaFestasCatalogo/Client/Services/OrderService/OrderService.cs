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

            if (result.IsSuccessStatusCode)
            {
                var orderDto = await result.Content.ReadFromJsonAsync<OrderDto>();
                return new ServiceResponse<OrderDto> { Success = true, Data = orderDto };
            }
            else
            {
                var errorResponse = await result.Content.ReadFromJsonAsync<ServiceResponse<OrderDto>>();
                return errorResponse ?? new ServiceResponse<OrderDto> { Success = false, Message = "Unknown error" };
            }
        }

    }
}
