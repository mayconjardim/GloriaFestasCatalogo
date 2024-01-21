global using Blazored.LocalStorage;
global using GloriaFestasCatalogo.Client.Services.AuthService;
global using GloriaFestasCatalogo.Client.Services.CartService;
global using GloriaFestasCatalogo.Client.Services.OrderService;
global using GloriaFestasCatalogo.Client.Services.ProductService;
global using GloriaFestasCatalogo.Client.Services.CategoryService;
global using GloriaFestasCatalogo.Client.Services.ConfigService;
global using Microsoft.AspNetCore.Components.Authorization;
global using Microsoft.AspNetCore.Components.Web;
global using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


namespace GloriaFestasCatalogo.Client
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");
			builder.RootComponents.Add<HeadOutlet>("head::after");

			builder.Services.AddScoped<IProductService, ProductService>();
			builder.Services.AddScoped<IOrderService, OrderService>();
			builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
			builder.Services.AddScoped<ICategoryService, CategoryService>();
			builder.Services.AddScoped<IAuthService, AuthService>();
			builder.Services.AddScoped<IConfigService, ConfigService>();
			builder.Services.AddScoped<CartService>();
			builder.Services.AddBlazorBootstrap();

			builder.Services.AddOptions();
			builder.Services.AddAuthorizationCore();
			builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
			builder.Services.AddBlazoredLocalStorage();

			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

			await builder.Build().RunAsync();
		}
	}
}
