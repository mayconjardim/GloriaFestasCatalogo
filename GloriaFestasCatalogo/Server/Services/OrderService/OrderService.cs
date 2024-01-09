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

		public async Task<ServiceResponse<bool>> DeleteOrder(int orderId)
		{
			try
			{
				var orderExists = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

				if (orderExists == null)
				{
					return new ServiceResponse<bool>
					{
						Success = false,
						Message = "Pedido não encontrado."
					};
				}
				else
				{
					_context.Orders.Remove(orderExists);
					await _context.SaveChangesAsync();

					return new ServiceResponse<bool> { Success = true, Data = true };
				}

			}
			catch (Exception ex)
			{
				return new ServiceResponse<bool>
				{
					Success = false,
					Message = ex.Message
				};
			}
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

		public async Task<ServiceResponse<OrderResponse>> GetOrderPageableAsync(int page, int pageSize = 20, string text = null, OrderStatus? status = null)
		{
			var query = _context.Orders.Include(p => p.Products).OrderByDescending(o => o.OrderDate);

			if (!string.IsNullOrEmpty(text))
			{
				query = (IOrderedQueryable<Order>)query.Where(o => o.Whatsapp.Contains(text) || EF.Functions.Like(o.Name, $"%{text}%"));
			}

			if (status.HasValue)
			{
				query = (IOrderedQueryable<Order>)query.Where(o => o.Status == status);
			}

			var totalOrders = await query.CountAsync();

			var orders = await query
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			var orderDtos = _mapper.Map<List<OrderDto>>(orders);

			if (orderDtos == null || orderDtos.Count == 0)
			{
				var emptyResponse = new ServiceResponse<OrderResponse>
				{
					Success = true,
					Data = new OrderResponse
					{
						Orders = new List<OrderDto>(),
						Pages = 0,
						CurrentPage = page
					},
					Message = "Não foram encontrados pedidos para os critérios fornecidos."
				};

				return emptyResponse;
			}

			var totalPages = (int)Math.Ceiling((double)totalOrders / pageSize);

			var response = new ServiceResponse<OrderResponse>
			{
				Data = new OrderResponse
				{
					Orders = orderDtos,
					Pages = totalPages,
					CurrentPage = page
				}
			};

			return response;
		}


		public async Task<ServiceResponse<OrderDto>> UpdateOrder(OrderDto orderDto)
		{
			var response = new ServiceResponse<OrderDto>();

			var order = await _context.Orders.FindAsync(orderDto.Id);

			if (order != null)
			{
				order.Status = orderDto.Status;
				await _context.SaveChangesAsync();
				response.Success = true;
				response.Message = "Status atualizado com sucesso!";
				return response;
			}

			response.Success = false;
			response.Message = "Pedido não encontrado!";
			return response;
		}

	}
}
