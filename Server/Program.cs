using Blazored.Toast;
using Blazored.Toast.Services;
using ManagingSalesApp.Server.DB;
using ManagingSalesApp.Server.Services;
using ManagingSalesApp.Server.Services.Interfaces;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Npgsql;

namespace ManagingSalesApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
			builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
           // string connection = builder.Configuration.GetConnectionString("DefaultConnection");
			string localConnection = builder.Configuration.GetConnectionString("FallBackConnection");
			string connection = builder.Configuration.GetConnectionString("NeonTech");

			builder.Services.AddDbContext<ApplicationContext>(options =>
			{
				try
				{

					options.UseNpgsql(connection);
				
					// Если соединение открыто успешно, можно установить настройки контекста
				}
				catch (Exception ex)
				{
					Console.WriteLine("Не удалось подключиться к первой базе данных: ");
					Console.WriteLine("Используем второе подключение...");
					options.UseSqlServer(localConnection, sqlOptions =>
					{
						sqlOptions.EnableRetryOnFailure(
							maxRetryCount: 5, // максимальное количество попыток подключения
							maxRetryDelay: TimeSpan.FromSeconds(30), // максимальная задержка между попытками
							errorNumbersToAdd: null // ошибки, которые должны вызывать повторные попытки
						);
					});
				}
			});
			//	builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection)); // подключение к бд
			builder.Services.AddTransient<IOrderService, OrderService>(); // транзит означает что будет создавать каждый раз при обращенини к этому сервису t
			builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();
			builder.Services.AddHttpClient();
			//builder.Services.AddSingleton<MyDataContext>();
			builder.Services.AddHttpContextAccessor();
            builder.Services.AddBlazoredToast();
            builder.Services.AddLogging();
            builder.Services.AddControllers();
            builder.Services.AddMemoryCache();
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
            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");
            app.Run();
        }
		public static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args)
			.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder
					.UseStartup<Program>();
			});
		// Метод настройки для окружения "Development"
		public static void ConfigureDevelopment(IApplicationBuilder app, IWebHostEnvironment env)
		{
			// Ваши настройки для разработки
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				// Дополнительные настройки для разработки
			}
		}
	}
}