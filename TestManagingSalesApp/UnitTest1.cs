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
using Microsoft.AspNetCore.Mvc;

namespace TestManagingSalesApp
{
    public class UnitTest1 : TestContext
    {

		[Fact]
		public void IndexViewDataMessage()
		{
			// Arrange ��������� �������
			//derController controller = new OrderController();

			// Act ��������� ����
			//ViewResult result = controller.Index() as ViewResult;

			// Assert ������������ ���������
			///Assert.Equal("Hello world!", result?.ViewData["Message"]);
		}


		[Fact]
        public void TestYourPageInitialization()
        {
            // Arrange
            using var ctx = new TestContext();

            // �������������� ���� � �������, ���� ����������
            var mockYourService = new Mock<DbContext>();
           // mockYourService.Setup(/* ��������� ����, ���� ����� */);

            // ������������ ���� � ������� � ���������� ������������
            ctx.Services.AddSingleton<DbContext>(mockYourService.Object);

            // ������� ���������
            var cut = ctx.RenderComponent<Counter>();

            // Act
            // ��������� ����� �������������� ��������, ���� ��� ����������

            // Assert
            // ��������� ����������, ��������� ��������� ����� �������������
            Assert.NotNull(cut.Instance.orders); // ������ �������� �������� ����� �������������

            // �������������� �������� � ����������� �� ������ ����������
        }



    }
}