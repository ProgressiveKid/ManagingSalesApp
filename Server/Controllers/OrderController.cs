using ManagingSalesApp.Server.DB;
using ManagingSalesApp.Server.Fuilters;
using ManagingSalesApp.Server.Services.Interfaces;
using ManagingSalesApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace ManagingSalesApp.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	//[ServiceFilter(typeof(LogRequestsFilter))]
	public class OrderController : Controller
	{
		private ApplicationContext db;
		private readonly IOrderService _orderService;
		private readonly IRabbitMqService _mqService;
		public OrderController(ApplicationContext context, IOrderService orderService, IRabbitMqService mqService)
		{
			_orderService = orderService;
			 db = context;
			_mqService = mqService;
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
	
			// Найти количество уникальных значений свойства Height
			int uniqueHeightCount = subElements
				.Select(subElement => subElement.Height) // Выбираем значения свойства Height
				.Distinct() // Оставляем только уникальные значения
				.Count(); // Подсчитываем количество уникальных значений

			// подсчёт уникальных кортежей (высота и ширина)
			int uniqueTupleCount = subElements
			.GroupBy(subElement => new { subElement.Width, subElement.Height })
				.Select(group => group.Key) // Выбираем только ключи (ширину и высоту)
				.Distinct()
				.Count();
			return $" кол-во уникальных кортежей {uniqueWidthElements}, кол-во элементов с уникальной высотой {uniqueHeightCount}";
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
			//LoggerMethod(nameOrder);
			return _orderService.GetOneOrder(nameOrder);
		}
		[HttpPost("CreateOrder")]
		public async Task<IActionResult> CreateOrder(Order order)
		{
			//LoggerMethod(order);
			Dictionary<bool, string> resultOfCreate = _orderService.CreateOrder(order);
			if (resultOfCreate.First().Key)
			{				
				string json = JsonConvert.SerializeObject(order);
				//_mqService.SendMessage(json); на время выключил работу с брокером сообщении
				return Ok(resultOfCreate.First().Value);
			}
			else
				return BadRequest(resultOfCreate.First().Value);
		}
		[HttpPost("CreateWindow")]
		public IActionResult CreateWindow(Window window)
		{
			Window createdWindow = _orderService.CreateWindow(window);
			return Ok(createdWindow);
		}
		[HttpPost("CreateSubElement")]
		public IActionResult CreateSubElement(SubElement subElement)
		{
			SubElement createdSubElement = _orderService.CreateSubElement(subElement);
			return Ok(createdSubElement);
		}
		[HttpPut("EditOrder")]
		public IActionResult EditOrder(Order order)
		{
			Order editedOrder = _orderService.EditOrder(order);
			return Ok(editedOrder);
		}
		[HttpPut("DeleteOrder")]
		public IActionResult DeleteOrder(Order order)
		{
			Dictionary<bool, string> resultOfDelete = _orderService.DeleteOrder(order);
			if (resultOfDelete.First().Key)
			{
				return Ok(resultOfDelete.First().Value);
			}
			else return BadRequest(resultOfDelete.First().Value);
		}
	}
}
