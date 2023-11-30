using GloriaFestasCatalogo.Shared.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace GloriaFestasCatalogo.Shared.Models.Orders
{
	public class Order
	{

		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Whatsapp { get; set; } = string.Empty;
		public string ZipCode { get; set; } = string.Empty;
		public string Street { get; set; } = string.Empty;
		public string Number { get; set; } = string.Empty;
		public string Neighborhood { get; set; } = string.Empty;
		public string City { get; set; } = string.Empty;
		public string State { get; set; } = string.Empty;
		public string Complement { get; set; } = string.Empty;
		public string Observation { get; set; } = string.Empty;
		public string PaymentMethod { get; set; } = string.Empty;
		public DateTime OrderDate { get; set; } = DateTime.Now;
		public OrderStatus Status { get; set; } = OrderStatus.ABERTO;
		[Column(TypeName = "decimal(18,2)")]
		public decimal TotalPrice { get; set; }
		public List<OrderCart> Products { get; set; }


	}
}
