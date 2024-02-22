//using Bunit;
//using ManagingSalesApp.Client.Pages;
//using Moq;
//using Microsoft.Extensions.DependencyInjection;
//using ManagingSalesApp.Server.DB;
//using ManagingSalesApp.Server.Controllers;
//using Microsoft.EntityFrameworkCore;
//using ManagingSalesApp.Server.Services.Interfaces;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using System.Net;
//using ManagingSalesApp.Shared;

//namespace TestManagingSalesApp
//{
//	public class UnitTest1 : TestContext
//    {

//		private readonly Mock<IOrderService> _mockOrderService;
//		private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
//		private readonly Mock<ILogger<OrderController>> _mockLogger;
//		private readonly OrderController _controller;
//		private readonly HttpClient _client;
//		public UnitTest1()
//		{
//			//_mockOrderService = new Mock<IOrderService>();
//			//_mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
//			//_mockLogger = new Mock<ILogger<OrderController>>();
//			//var dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
//			//.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=managingDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False")
//			//.Options;
//			//var realDbContext = new ApplicationContext();
//			//_controller = new OrderController(
//			//	context: realDbContext, // You can pass null because we're not testing database interaction in this example
//			//	httpContextAccessor: _mockHttpContextAccessor.Object,
//			//	logger: _mockLogger.Object,
//			//	orderService: _mockOrderService.Object
//			//);

//			//var server = new TestServer(new WebHostBuilder().UseStartup<Program>()
//			//	.UseEnvironment("Development"));

//			////_client = server.CreateClient();

//		}

//		[Theory]
//		[InlineData("GET")]
//		public async Task GetALlThings(string aaa)
//		{
//			//var request = new HttpRequestMessage(new HttpMethod(HttpMethod.Get.Method), "Order/GetAllOrders");

//			//var response = await _client.SendAsync(request);
//			//response.EnsureSuccessStatusCode();
//			//Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		
//		}




//		[Fact]
//		public void IndexViewDataMessage()
//		{
//			var ordersList = new List<Order>
//			{
//				new Order
//				{
//					Id = 1,
//					Name = "New York Building 1",
//					State = "NY",
//					Windows = new List<Window>
//					{
//						new Window
//						{
//							Id = 2,
//							Name = "C Zone 5",
//							QuantityOfWindows = 2,
//							TotalSubElements = 1,
//							OrderId = 1
//						},
//						new Window
//						{
//							Id = 3,
//							Name = "C Zone 5",
//							QuantityOfWindows = 2,
//							TotalSubElements = 1,
//							OrderId = 1
//						},


//					}


//				},
//				new Order
//				{
//					Id = 2,
//					Name = "California Hotel AJK",
//					State = "CA",
//					Windows = new List<Window>
//					{
//						new Window
//						{
//							Id = 4,
//							Name = "C Zone 4",
//							QuantityOfWindows = 2,
//							TotalSubElements = 1,
//							OrderId = 2,
//							SubElements = new List<SubElement>
//							{
//								new SubElement
//								{
//									Id = 3,
//									Type = "Doors",
//									Width = 1200,
//									Height = 1850,
//									WindowId = 4
//								},
//								new SubElement
//								{
//									Id = 1,
//									Type = "Window",
//									Width = 800,
//									Height = 1850,
//									WindowId = 4
//								},
//								// ������ �������� SubElement
//							}

//						},
//						new Window
//						{
//							Id = 5,
//							Name = "C Zone 3",
//							QuantityOfWindows = 2,
//							TotalSubElements = 1,
//							OrderId = 2,
//							SubElements = new List<SubElement>
//							{
//								new SubElement
//								{
//									Id = 2,
//									Type = "Doors",
//									Width = 1200,
//									Height = 1850,
//									WindowId = 5
//								},
							
//								// ������ �������� SubElement
//							}
//						},


//					}
//				}
//			};

//			var mockContext = new Mock<ApplicationContext>();
//			var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
//			var mockLogger = new Mock<ILogger<OrderController>>();
//			var mockOrderService = new Mock<IOrderService>();
//			var iHttpClientFactory = new Mock<IHttpClientFactory>();
//			mockOrderService.Setup(service => service.GetAllOrders())
//							.Returns(ordersList);

//			// �������� ����������� � ����������� ������� �������
//			var controller = new OrderController(
//				mockContext.Object,
//				mockHttpContextAccessor.Object,
//				mockLogger.Object,
//				mockOrderService.Object,
//				iHttpClientFactory.Object,
				
//			);

//			// ����� ������ �����������
//			var orders = controller.GetAllOrders();
//			// �������� �� null
//			Assert.NotNull(orders);

//			// �������� �� ���������� ���������
//			Assert.NotEmpty(orders);
			
//			// �������� �� ���������� ���������, ������ ����
//			Assert.NotEqual(0, orders.Count()); // ���� ���������, ��� ������ �� ������
//												//Assert.Equal(1200, 1200);
//												// Arrange ��������� �������
//												//derController controller = new OrderController();

//			// Act ��������� ����
//			//ViewResult result = controller.Index() as ViewResult;

//			// Assert ������������ ���������
//			///Assert.Equal("Hello world!", result?.ViewData["Message"]);
//		}



//		//[Fact]
//		//public void CreateOrder_AddsOrderToDatabase()
//		//{
//		//	// Arrange
//		//	var options = new DbContextOptionsBuilder<ApplicationContext>()
//		//   .UseInMemoryDatabase(databaseName: "TestDatabase")
//		//   .Options;
//		//	var dbContext = new ApplicationContext(options);
//		////	var mockContext = new Mock<ApplicationContext>();
//		//	var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
//		//	var mockLogger = new Mock<ILogger<OrderController>>();
//		//	var mockOrderService = new Mock<IOrderService>();
//		//	//mockOrderService.Setup(service => service.GetAllOrders())
//		//	//				.Returns(ordersList);

//		//	// �������� ����������� � ����������� ������� �������
//		//	var controller = new OrderController(
//		//		dbContext,
//		//		mockHttpContextAccessor.Object,
//		//		mockLogger.Object,
//		//		mockOrderService.Object
//		//	);

			

//		//	var order = new Order { Name = "NewOrder", /* ������ �������� ������� */ };

//		//	// Act
//		//	var result = controller.CreateOrder(order);

//		//	// Assert
//		//	Assert.True(result); // ���������, ��� ��������� �������� �������
//		//	Assert.Equal(1, dbContext.Orders.Count()); // ���������, ��� ������ ��� �������� � ���� ������
//		//	var createdOrder = dbContext.Orders.Single();
//		//	Assert.Equal(order.Name, createdOrder.Name); // ���������, ��� ������ ������� ������������� ��������� ������
//		//												 // ��������� ������ �������� �������, ���� ��� ����������
//		//}

//		[Fact]
//		public void CreateOrder_EmptyName_ReturnsErrorMessage()
//		{
//			// Arrange
//			var mockContext = new Mock<ApplicationContext>();
//			var mockHttpContextAccessor = new Mock<IHttpContextAccessor>(); var mockHttpContext = new Mock<HttpContext>();
//			var mockLogger = new Mock<ILogger<OrderController>>();
//			var mockConnection = new Mock<ConnectionInfo>();
//			var iHttpClientFactory = new Mock<IHttpClientFactory>();
//			mockConnection.SetupGet(c => c.RemoteIpAddress).Returns(IPAddress.Parse("127.0.0.1")); // ������ IP-������ ��� �����
//			mockHttpContext.SetupGet(c => c.Connection).Returns(mockConnection.Object);
//			mockHttpContextAccessor.SetupGet(a => a.HttpContext).Returns(mockHttpContext.Object);

//			var mockOrderService = new Mock<IOrderService>();
			 
//			var controller = new OrderController(
//			mockContext.Object,
//			mockHttpContextAccessor.Object,
//			mockLogger.Object,
//			mockOrderService.Object,
//			iHttpClientFactory.Object);
			

//			// Act
//			var result = controller.CreateOrder(new Order { Name = "test" });

//			// Assert
			

//		}

//		[Fact]
//        public void TestYourPageInitialization()
//        {
//            // Arrange
//            using var ctx = new TestContext();

//            // �������������� ���� � �������, ���� ����������
//            var mockYourService = new Mock<DbContext>();
//           // mockYourService.Setup(/* ��������� ����, ���� ����� */);

//            // ������������ ���� � ������� � ���������� ������������
//            ctx.Services.AddSingleton<DbContext>(mockYourService.Object);

//            // ������� ���������
//            var cut = ctx.RenderComponent<Counter>();

//            // Act
//            // ��������� ����� �������������� ��������, ���� ��� ����������

//            // Assert
//            // ��������� ����������, ��������� ��������� ����� �������������
//            Assert.NotNull(cut.Instance.orders); // ������ �������� �������� ����� �������������

//            // �������������� �������� � ����������� �� ������ ����������
//        }



//    }
//}