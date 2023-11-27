using GloriaFestasCatalogo.Shared.Dtos.Orders;
using GloriaFestasCatalogo.Shared.Utils;

namespace GloriaFestasCatalogo.Server.Services.OrderService
{
    public interface IOrderService
    {

        Task<ServiceResponse<OrderDto>> CreateOrder(OrderCreateDto order);
        Task<ServiceResponse<OrderDto>> GetOrderById(int orderId);

    }
}
