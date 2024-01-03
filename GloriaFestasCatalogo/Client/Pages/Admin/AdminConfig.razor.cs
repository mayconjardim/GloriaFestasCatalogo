using BlazorBootstrap;
using GloriaFestasCatalogo.Shared.Dtos.Config;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.RegularExpressions;

namespace GloriaFestasCatalogo.Client.Pages.Admin
{
	partial class AdminConfig
	{

		private AppConfigDto appConfig;
		private string newNumber;
		private string message = string.Empty;

		[Inject] protected ToastService toastService { get; set; }

		protected override async Task OnInitializedAsync()
		{
			message = "Carregando Configurações...";

			var result = await ConfigService.GetConfig();
			if (!result.Success)
			{
				message = result.Message;
			}
			else
			{
				appConfig = result.Data;
				newNumber = appConfig.PhoneNumber;
			}
		}

		private async Task EditConfig()
		{
			if (newNumber != null)
			{
				if (Regex.IsMatch(newNumber, @"^55\d{11}$"))
				{

					var result = await ConfigService.GetConfig();
					if (result.Success)
					{
						await InvokeAsync(() =>
						{
							StateHasChanged();
							toastService.Notify(new(ToastType.Success, $"Configuração editada com sucesso!"));
						});

						await CloseModal("EditModal");
					}
				}
				else
				{
					toastService.Notify(new(ToastType.Danger, $"Por favor, insira um número de telefone válido no formato 5521999990000."));
				}
			}
		}

		private async Task OpenModal(string modal, int id)
		{

			var result = await ConfigService.GetConfig();

			if (!result.Success)
			{
				message = result.Message;
			}
			else
			{
				appConfig = result.Data;
			}

			await JSRuntime.InvokeAsync<object>("openModal", modal);
		}

		private async Task CloseModal(string modal)
		{
			await JSRuntime.InvokeAsync<object>("closeModal", modal);
		}

	}
}
