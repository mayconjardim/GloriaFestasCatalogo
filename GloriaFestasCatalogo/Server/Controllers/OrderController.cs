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

		[HttpGet("page/{page}")]
		public async Task<ActionResult<ServiceResponse<OrderResponse>>> GetProductsPageable(int page, int pageSize = 20, string? text = null, OrderStatus? status = null)
		{
			try
			{
				var result = await _orderService.GetOrderPageableAsync(page, pageSize, text, status);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Erro interno no servidor.");
			}
		}


		[HttpPut]
		public async Task<ActionResult<ServiceResponse<bool>>> UpdateOrderStatus(OrderDto order)
		{
			var response = await _orderService.UpdateOrder(order);

			if (response.Success)
			{
				return Ok(response);
			}
			else
			{
				return BadRequest(response);
			}
		}


	}
}
