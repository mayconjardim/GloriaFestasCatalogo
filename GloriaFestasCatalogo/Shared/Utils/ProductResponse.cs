using GloriaFestasCatalogo.Shared.Dtos.Orders;

namespace GloriaFestasCatalogo.Shared.Utils
{
    public class OrderResponse
    {

        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }

    }
}
