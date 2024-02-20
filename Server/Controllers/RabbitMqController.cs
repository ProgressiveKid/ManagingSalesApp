using ManagingSalesApp.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ManagingSalesApp.Server.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class RabbitMqController : Controller
	{
		private readonly IRabbitMqService _mqService;

		public RabbitMqController(IRabbitMqService mqService)
		{
			_mqService = mqService;
		}

		[Route("SendMessage")]
		[HttpGet]
		public IActionResult SendMessage(string message)
		{
			_mqService.SendMessage(message);

			return Ok("Сообщение отправлено");
		}
	}
}
