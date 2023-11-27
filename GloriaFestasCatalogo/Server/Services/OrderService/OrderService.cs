using AutoMapper;
using GloriaFestasCatalogo.Server.Data;
using GloriaFestasCatalogo.Shared.Dtos.Orders;
using GloriaFestasCatalogo.Shared.Models.Orders;
using GloriaFestasCatalogo.Shared.Utils;
using Microsoft.EntityFrameworkCore;
namespace GloriaFestasCatalogo.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {

        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public OrderService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<OrderDto>> CreateOrder(OrderCreateDto order)
        {
            ServiceResponse<OrderDto> response = new ServiceResponse<OrderDto>();

            var newOrder = _mapper.Map<Order>(order);

            List<OrderCart> carts = new List<OrderCart>();

            foreach (var product in order.Products)
            {

                carts.Add(new OrderCart
                {
                    Order = newOrder,
                    ProductId = product.Product.Id,
                    Quantity = product.Quantity
                });

            }

            newOrder.Products = carts;

            try
            {
                _context.Add(newOrder);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            response.Data = _mapper.Map<OrderDto>(newOrder);
            return response;
        }

        public async Task<ServiceResponse<OrderDto>> GetOrderById(int orderId)
        {
            var response = new ServiceResponse<OrderDto>();
            var order = await _context.Orders
            .Include(o => o.Products)
            .ThenInclude(pc => pc.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                response.Success = false;
                response.Message = $"O pedido com o Id {orderId} não existe!";
            }
            else
            {
                response.Data = _mapper.Map<OrderDto>(order);
            }

            return response;
        }


    }
}
