using AutoMapper;
using GloriaFestasCatalogo.Server.Data;
using GloriaFestasCatalogo.Shared.Dtos.Orders;
using GloriaFestasCatalogo.Shared.Models.Orders;
using GloriaFestasCatalogo.Shared.Utils;
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


    }
}
