using ManagingSalesApp.Server.Services.Interfaces;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace ManagingSalesApp.Server.Services
{
	public class RabbitMqService : IRabbitMqService
	{
		public void SendMessage(object obj)
		{
			var message = JsonSerializer.Serialize(obj);
			SendMessage(message);
		}

		public void SendMessage(string message)
		{

			//var factory = new ConnectionFactory() { Uri = new Uri("amqps://cvctidtm:EJB2M3vCGHNmVsCwdtcjctRGrY35mtX5@woodpecker.rmq.cloudamqp.com/cvctidtm") };
			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queue: "MyQueue",
							   durable: false,
							   exclusive: false,
							   autoDelete: false,
							   arguments: null);

				var body = Encoding.UTF8.GetBytes(message);

				channel.BasicPublish(exchange: "",
							   routingKey: "MyQueue",
							   basicProperties: null,
							   body: body);
			}
		}
	}
}
