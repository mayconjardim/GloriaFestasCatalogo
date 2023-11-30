using GloriaFestasCatalogo.Shared.Dtos.Orders;
using GloriaFestasCatalogo.Shared.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GloriaFestasCatalogo.Client.Pages.Admin
{
	partial class OrderList
	{

		private OrderResponse orders;
		private OrderDto selectedOrder;
		private string message = string.Empty;
		private int currentPage = 1;
		private int pageSize = 20;
		private OrderStatus selectedStatus;
		private int selectedStatusValue = 0;
		private string selectedValue;

		protected override async Task OnInitializedAsync()
		{
			message = "Carregando Pedidos...";

			var result = await OrderService.GetOrderPageableAsync(currentPage, pageSize);
			if (!result.Success)
			{
				message = result.Message;
			}
			else
			{
				orders = result.Data;
				currentPage = result.Data.CurrentPage;
			}
		}

		private async Task OrdersByStatus(int value)
		{
			string text = string.Empty;
			OrderStatus status = OrderStatus.ABERTO;

			if (value == 1)
			{
				status = OrderStatus.ABERTO;
			}
			else if (value == 2)
			{
				status = OrderStatus.PROCESSANDO;

			}
			else if (value == 3)
			{
				status = OrderStatus.FECHADO;

			}
			else if (value == 4)
			{
				status = OrderStatus.CANCELADO;
			}

			var result = await OrderService.GetOrderPageableAsync(currentPage, pageSize, text, status);

			if (!result.Success)
			{
				message = result.Message;
				StateHasChanged();
			}
			else
			{
				orders = result.Data;
				StateHasChanged();
			}
		}

		private async Task ChangePage(int nextPage)
		{

			var result = await OrderService.GetOrderPageableAsync(nextPage, pageSize);

			if (!result.Success)
			{
				message = result.Message;
			}
			else
			{
				orders = result.Data;
				currentPage = result.Data.CurrentPage;
			}
		}

		private async Task OpenModal(string modal, int id)
		{

			var result = await OrderService.GetOrderById(id);

			if (!result.Success)
			{
				message = result.Message;
			}
			else
			{
				selectedOrder = result.Data;
				selectedStatus = selectedOrder.Status;
			}

			await JSRuntime.InvokeAsync<object>("openModal", modal);
		}

		private async Task CloseModal(string modal)
		{
			await JSRuntime.InvokeAsync<object>("closeModal", modal);
		}

		private async Task Print()
		{
			await JSRuntime.InvokeVoidAsync("printDiv", "PrintModal");

		}

		private List<OrderStatusOption> GetOrderStatusOptions()
		{
			var options = new List<OrderStatusOption>();

			foreach (OrderStatus status in Enum.GetValues(typeof(OrderStatus)))
			{
				var option = new OrderStatusOption
				{
					Value = (int)status,
					Text = status.ToString(),
					IsSelected = (status == selectedStatus)
				};

				options.Add(option);
			}

			return options;
		}

		public static string GetBadgeClass(OrderStatus status)
		{
			switch (status)
			{
				case OrderStatus.ABERTO:

					return "badge text-bg-success";

				case OrderStatus.PROCESSANDO:
					return "badge text-bg-warning";

				case OrderStatus.FECHADO:
					return "badge text-bg-secondary";

				case OrderStatus.CANCELADO:
					return "badge text-bg-danger";

				default:
					return "badge text-bg-primary";
			}
		}

		private void HandleSelectChange(ChangeEventArgs e)
		{
			selectedValue = e.Value.ToString();

			if (selectedValue == "1")
			{
				selectedOrder.Status = OrderStatus.ABERTO;
			}
			else if (selectedValue == "2")
			{
				selectedOrder.Status = OrderStatus.PROCESSANDO;

			}
			else if (selectedValue == "3")
			{
				selectedOrder.Status = OrderStatus.FECHADO;

			}
			else if (selectedValue == "4")
			{
				selectedOrder.Status = OrderStatus.CANCELADO;
			}

		}

		private async Task HandleStatusChange(ChangeEventArgs e)
		{
			if (int.TryParse(e.Value.ToString(), out var value))
			{
				selectedStatusValue = value;
				await OrdersByStatus(selectedStatusValue);
			}
		}

		private async Task UpdateStatus()
		{

			var result = await OrderService.UpdateOrder(selectedOrder);

			if (!result.Success)
			{
				message = result.Message;
			}
			else
			{
				selectedOrder = result.Data;
			}
			RefreshPage();
		}

		private void RefreshPage()
		{
			NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
		}
	}
}
