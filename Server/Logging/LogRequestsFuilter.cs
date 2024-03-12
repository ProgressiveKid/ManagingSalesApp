using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
namespace ManagingSalesApp.Server.Fuilters
{
	public class LogRequestsFilter : IActionFilter
	{
		private readonly ILogger _logger;

		public LogRequestsFilter(ILogger<LogRequestsFilter> logger)
		{
			_logger = logger;
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			// Логирование входящего запроса
			var request = context.HttpContext.Request;
			var requestBody = JsonConvert.SerializeObject(context.ActionArguments);
			_logger.LogInformation($"Incoming request: {request.Method} {request.Path} - Body: {requestBody}");
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
			// Ничего не делаем после выполнения действия
		}
	}
}
