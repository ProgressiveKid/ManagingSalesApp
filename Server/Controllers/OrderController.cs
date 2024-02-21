using Blazored.Toast.Services;
using ManagingSalesApp.Server.DB;
using ManagingSalesApp.Server.Services.Interfaces;
using ManagingSalesApp.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json;

namespace ManagingSalesApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        ApplicationContext db;
		private IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<OrderController> _logger;
		private readonly IHttpClientFactory _clientFactory;
		private readonly IRabbitMqService _mqService;
		public OrderController(ApplicationContext context, IHttpContextAccessor httpContextAccessor, ILogger<OrderController> logger, IOrderService orderService, IHttpClientFactory clientFactory, IRabbitMqService mqService)
        {
            _orderService = orderService;
            db = context;
          
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
			_clientFactory = clientFactory;
			_mqService = mqService;
			Console.WriteLine("Я конструктор и я вызвался");
        }
		[HttpGet("GetCountOfEniqueEl")]
		public string GetCountOfEniqueEl()
        {
			List<SubElement> subElements = db.SubElements.ToList();
            // Найти количество уникальных значений свойства Width
            var uniqueWidthElements = subElements
                 .GroupBy(subElement => new { subElement.Height, subElement.Width }) // Группировка по значению Width
                 .Where(group => group.Count() == 1); // Выбрать только группы с одним элементом
				// .SelectMany(group => group).ToList();
			var myUnique = subElements.GroupBy(subElem => new { subElem.Width, subElem.Height })
				.Where(u => u.Count() == 1).SelectMany(u => u)
				.ToList();
			// Найти количество уникальных значений свойства Height
			int uniqueHeightCount = subElements
				.Select(subElement => subElement.Height) // Выбираем значения свойства Height
				.Distinct() // Оставляем только уникальные значения
				.Count(); // Подсчитываем количество уникальных значений
            return $"Height + {uniqueHeightCount} + Width = {uniqueWidthElements}";
		}
		[HttpGet("GetAllOrders")]
        public IEnumerable<Order> GetAllOrders()
        {
            return _orderService.GetAllOrders();
        }
        [HttpGet("GetOrdersName")]
        public IEnumerable<string> GetOrdersName()
        {
            return _orderService.GetOrdersName();
        }
        [HttpGet("GetOneOrder")]
        public IEnumerable<Order> GetOneOrder(string nameOrder)
        {
            LoggerMethod(nameOrder);
            return _orderService.GetOneOrder(nameOrder);
        }
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            LoggerMethod(order);
            Dictionary<bool, string> resultOfCreate = _orderService.CreateOrder(order);
            if (resultOfCreate.First().Key)
            {
                _mqService.SendMessage($"Создался новый заказ - выезжайте на {order.State} монтаж");
				return Ok(resultOfCreate.First().Value);

			} else
            return BadRequest(resultOfCreate.First().Value);
        }
        [HttpPost("CreateWindow")]
        public IActionResult CreateWindow(Window window)
        {
            LoggerMethod(window);
           Window createdWindow = _orderService.CreateWindow(window);
            return Ok(createdWindow);
        }
        [HttpPost("CreateSubElement")]
        public IActionResult CreateSubElement(SubElement subElement)
        {
            LoggerMethod(subElement);
			SubElement createdSubElement = _orderService.CreateSubElement(subElement);
			return Ok(createdSubElement);
		}
        [HttpPut("EditOrder")]
        public IActionResult EditOrder(Order order)
        {
            LoggerMethod(order);
            Order editedOrder = _orderService.EditOrder(order);
            return Ok(editedOrder);
        }
        [HttpPut("DeleteOrder")]
        public IActionResult DeleteOrder(Order order)
        {
            LoggerMethod(order);
			Dictionary<bool, string> resultOfDelete = _orderService.DeleteOrder(order);
            if (resultOfDelete.First().Key)
            {
				return Ok(resultOfDelete.First().Value);
			}
            else return BadRequest(resultOfDelete.First().Value);
		}
        public void LoggerMethod(object logMess)
        {
            var ipAddres = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
            _logger.LogInformation($"Ip Addres: {ipAddres} \n Received data: {JsonConvert.SerializeObject(logMess)}");
        }
    }
}
