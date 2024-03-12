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
using Microsoft.Extensions.Configuration;
using ManagingSalesApp.Server.Fuilters;
using ManagingSalesApp.Server.Logging;

namespace ManagingSalesApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
			// string connection = builder.Configuration.GetConnectionString("DefaultConnection");
			string localConnection = builder.Configuration.GetConnectionString("FallBackConnection");
			string connection = builder.Configuration.GetConnectionString("NeonTech");
			bool isFirstConnection = true;
			using (var connectionposqtre = new NpgsqlConnection(connection))
			{
				try
				{
					connectionposqtre.Open();
					if (connectionposqtre.State != System.Data.ConnectionState.Open)
					{
						isFirstConnection = false;
					}
				}
				catch (Exception ex)
				{
					isFirstConnection = false;
				}
			}
			builder.Services.AddDbContext<ApplicationContext>(options =>
			{
				if (isFirstConnection)
				{
					options.UseNpgsql(connection);
					Console.WriteLine("Hello from connection");
				}
				else
				{
					
					options.UseSqlServer(localConnection);
				}
			});
			//builder.Services.AddScoped<LogRequestsFilter>();
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
		//	app.UseMiddleware<ResponseLoggingMiddleware>(); // логирование

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