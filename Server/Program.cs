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
            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
			string localConnection = builder.Configuration.GetConnectionString("FallBackConnection");
			//builder.Services.AddDbContext<ApplicationContext>(options =>
			//{
			//	options.UseSqlServer(connection, sqlOptions =>
			//	{
			//		sqlOptions.EnableRetryOnFailure(
			//			maxRetryCount: 5, // ������������ ���������� ������� �����������
			//			maxRetryDelay: TimeSpan.FromSeconds(2), // ������������ �������� ����� ���������
			//			errorNumbersToAdd: null // ������, ������� ������ �������� ��������� �������
			//		);
			//	});
			//});
			//builder.Services.AddDbContext<ApplicationContext>(options =>
			//{
			//	options.UseSqlServer(localConnection, sqlOptions =>
			//	{
			//		sqlOptions.EnableRetryOnFailure(
			//			maxRetryCount: 5, // ������������ ���������� ������� �����������
			//			maxRetryDelay: TimeSpan.FromSeconds(30), // ������������ �������� ����� ���������
			//			errorNumbersToAdd: null // ������, ������� ������ �������� ��������� �������
			//		);
			//	});
			//});
			
			builder.Services.AddDbContext<ApplicationContext>(options =>
			{
				try
				{
					options.UseSqlServer(connection, sqlOptions =>
					{
						sqlOptions.EnableRetryOnFailure(
							maxRetryCount: 5, // ������������ ���������� ������� �����������
							maxRetryDelay: TimeSpan.FromSeconds(2), // ������������ �������� ����� ���������
							errorNumbersToAdd: null // ������, ������� ������ �������� ��������� �������
						);
					});
					using (var context = new ApplicationContext())
					{
						context.Database.OpenConnection(); // ������� ������� ����������
					}
					// ���� ���������� ������� �������, ����� ���������� ��������� ���������
				}
				catch (Exception ex)
				{
					Console.WriteLine("�� ������� ������������ � ������ ���� ������: ");
					Console.WriteLine("���������� ������ �����������...");
					options.UseSqlServer(localConnection, sqlOptions =>
					{
						sqlOptions.EnableRetryOnFailure(
							maxRetryCount: 5, // ������������ ���������� ������� �����������
							maxRetryDelay: TimeSpan.FromSeconds(30), // ������������ �������� ����� ���������
							errorNumbersToAdd: null // ������, ������� ������ �������� ��������� �������
						);
					});
				}
			});
			//	builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection)); // ����������� � ��
			builder.Services.AddTransient<IOrderService, OrderService>(); // ������� �������� ��� ����� ��������� ������ ��� ��� ���������� � ����� ������� t
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
		// ����� ��������� ��� ��������� "Development"
		public static void ConfigureDevelopment(IApplicationBuilder app, IWebHostEnvironment env)
		{
			// ���� ��������� ��� ����������
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				// �������������� ��������� ��� ����������
			}
		}
	}
}