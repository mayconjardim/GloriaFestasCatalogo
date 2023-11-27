using GloriaFestasCatalogo.Server.Services.OrderService;
using GloriaFestasCatalogo.Shared.Dtos.Orders;
using GloriaFestasCatalogo.Shared.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GloriaFestasCatalogo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<OrderDto>>> CreatePlayer(OrderCreateDto request)
        {
            return Ok(await _orderService.CreateOrder(request));
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<ServiceResponse<OrderDto>>> GetOrderById(int orderId)
        {

            var result = await _orderService.GetOrderById(orderId);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);

        }

    }
}
