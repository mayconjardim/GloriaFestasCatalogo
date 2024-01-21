global using GloriaFestasCatalogo.Server.Services.AuthService;
using GloriaFestasCatalogo.Server.Data;
using GloriaFestasCatalogo.Server.Services.CategoryService;
using GloriaFestasCatalogo.Server.Services.ConfigService;
using GloriaFestasCatalogo.Server.Services.OrderService;
using GloriaFestasCatalogo.Server.Services.ProductService;
using GloriaFestasCatalogo.Server.Services.ProductTypeService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GloriaFestasCatalogo
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddDbContext<DataContext>(DbContextOptions =>
			DbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:GloriaDbConnect"]));

			builder.Services.AddControllersWithViews();
			builder.Services.AddRazorPages();

			builder.Services.AddScoped<IAuthService, AuthService>();
			builder.Services.AddScoped<IConfigService, ConfigService>();
			builder.Services.AddScoped<IProductService, ProductService>();
			builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
			builder.Services.AddScoped<IOrderService, OrderService>();
			builder.Services.AddScoped<ICategoryService, CategoryService>();

			builder.Configuration.AddUserSecrets<Program>();

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey =
							new SymmetricSecurityKey(System.Text.Encoding.UTF8
							.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
						ValidateIssuer = false,
						ValidateAudience = false
					};
				});
			builder.Services.AddHttpContextAccessor();

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gloria API", Version = "v1" });
			});
			builder.Services.AddAutoMapper(typeof(Program).Assembly);
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseWebAssemblyDebugging();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
			});
			app.UseHttpsRedirection();

			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapRazorPages();
			app.MapControllers();
			app.MapFallbackToFile("index.html");

			app.Run();
		}
	}
}
