using AutoMapper;
using GloriaFestasCatalogo.Shared.Dtos.Orders;
using GloriaFestasCatalogo.Shared.Models.Orders;
using GloriaFestasCatalogo.Shared.Models.Products;

namespace GloriaFestasCatalogo.Server.Profiles
{
    public class OrderProductsResolver : IValueResolver<OrderDto, Order, List<OrderCart>>
    {
        public List<OrderCart> Resolve(OrderDto source, Order destination, List<OrderCart> destMember, ResolutionContext context)
        {
            var orderCarts = source.Products.Select(productDto => new OrderCart
            {
                Product = new Product
                {
                    Name = productDto.ProductName,
                    Price = productDto.ProductPrice
                },
                Quantity = productDto.Quantity
            }).ToList();

            return orderCarts;
        }
    }
}
