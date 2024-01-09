using GloriaFestasCatalogo.Shared.Dtos.Orders;
using GloriaFestasCatalogo.Shared.Utils;

namespace GloriaFestasCatalogo.Server.Services.OrderService
{
    public interface IOrderService
    {

        Task<ServiceResponse<OrderResponse>> GetOrderPageableAsync(int page, int pageSize = 20, string text = null, OrderStatus? status = null);
        Task<ServiceResponse<OrderDto>> CreateOrder(OrderCreateDto order);
        Task<ServiceResponse<OrderDto>> GetOrderById(int orderId);
        Task<ServiceResponse<OrderDto>> UpdateOrder(OrderDto orderDto);
        Task<ServiceResponse<bool>> DeleteOrder(int orderId);

    }
}
