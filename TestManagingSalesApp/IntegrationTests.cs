using System;
using System.Net.Http;
using Xunit;
using ManagingSalesApp; // За
using Microsoft.AspNetCore.Mvc.Testing;

namespace TestManagingSalesApp
{
	public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly WebApplicationFactory<Program> _factory;

		public IntegrationTests(WebApplicationFactory<Program> factory)
		{
			_factory = factory;
		}

		[Fact]
		public async Task Get_All_Things_Returns_Success_Status_Code()
		{
			// Создаем HTTP-клиент для взаимодействия с тестовым сервером
			var client = _factory.CreateClient();

			// Отправляем GET-запрос на определенный эндпоинт вашего приложения
			var response = await client.GetAsync("/api/Order/GetAllOrders");

			// Проверяем, что ответ имеет успешный статус код (200 OK)
			response.EnsureSuccessStatusCode();
		}
	}
}
