using GloriaFestasCatalogo.Shared.Dtos.Orders;

namespace GloriaFestasCatalogo.Client.Shared
{
    public static class AppConstants
    {

        public static string Message(OrderDto order)
        {
            string message = string.Empty;

            if (order != null)
            {
                message = $"Olá Glória Festas, meu nome é {order.Name} 😀 \n" +
                          "\n" +
                          $"🛒 *Acabei de realizar o pedido abaixo:*\n" +
                          $"📝 Nº do Pedido: {order.Id} \n" +
                          "------\n" +
                          "🧾 Itens:\n" +
                          "\n";

                if (order.Products != null)
                {
                    foreach (var prod in order.Products)
                    {
                        if (prod != null)
                        {
                            message += $"{prod.Quantity}x {prod.ProductName} \n" +
                                       "\n";
                        }
                    }
                }

                message += "------\n" +
                           "\n" +
                           $"💸 Valor total do pedido: R$ {order.TotalPrice.ToString("N2")} + frete \n" +
                           $"💰 Forma de pagamento: {order.PaymentMethod}\n" +
                           "\n" +
                           $"🏠 Endereço: {order.Street}, Nº: {order.Number} \n" +
                           $"🌍 Cidade e CEP: {order.City} - {order.ZipCode}\n" +
						   $"❗ Informações adicionais: \n" +
						   $"{order.Observation} \n" +
						   "\n" +
                           $"Pedido feito em {order.OrderDate:dd/MM/yyyy} às {order.OrderDate:HH:mm}\n"
                           ;
            }

            return message;
        }

    }
}
