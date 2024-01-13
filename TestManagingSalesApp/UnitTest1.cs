using System;
using static System.Net.WebRequestMethods;
using ManagingSalesApp;
using Bunit;
using Xunit;
using ManagingSalesApp.Client.Pages;
using ManagingSalesApp.Client;
using ManagingSalesApp.Shared;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using ManagingSalesApp.Server.DB;
using ManagingSalesApp.Server.Controllers;
using Microsoft.EntityFrameworkCore;

namespace TestManagingSalesApp
{
    public class UnitTest1 : TestContext
    {
        [Fact]
        public void TestYourPageInitialization()
        {
            // Arrange
            using var ctx = new TestContext();

            // Подготавливаем моки и сервисы, если необходимо
            var mockYourService = new Mock<DbContext>();
           // mockYourService.Setup(/* настройка мока, если нужно */);

            // Регистрируем моки и сервисы в контейнере зависимостей
            ctx.Services.AddSingleton<DbContext>(mockYourService.Object);

            // Создаем компонент
            var cut = ctx.RenderComponent<Counter>();

            // Act
            // Выполняем любые дополнительные действия, если это необходимо

            // Assert
            // Проверяем результаты, ожидаемые изменения после инициализации
            Assert.NotNull(cut.Instance.orders); // Пример проверки свойства после инициализации

            // Дополнительные проверки в зависимости от вашего компонента
        }



    }
}