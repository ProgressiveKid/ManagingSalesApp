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