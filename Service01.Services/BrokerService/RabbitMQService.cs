using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Service01.Models.Models;
using System.Text;

namespace Service01.Services.BrokerService
{
	public class RabbitMQService : IBrokerService
	{
		private ConnectionFactory _factory {  get; set; }
		private ILogger<RabbitMQService> _logger { get; set; }

		public RabbitMQService(ILogger<RabbitMQService> logger)
		{
			_logger = logger;

			var factory = new ConnectionFactory()
			{
				HostName = "localhost",
				Port = AmqpTcpEndpoint.DefaultAmqpSslPort,
				VirtualHost = "/",
				UserName = "",
				Password = ""
			};
		}
		public async Task<RateResponseModel> GetRateAsync(RateRequestModel request)
		{
			var result = new RateResponseModel();

			try
			{
				using (var connection = await _factory.CreateConnectionAsync())
				using (var channel = await connection.CreateChannelAsync())
				{
					await channel.QueueDeclareAsync(queue: "rate",
										 durable: false,
										 exclusive: false,
										 autoDelete: false,
										 arguments: null);

					string message = JsonConvert.SerializeObject(request);
					var body = Encoding.UTF8.GetBytes(message);
					await channel.BasicPublishAsync(exchange: "",
										 routingKey: "key",
										 body: body);

					_logger.LogInformation($"Sent message: {message}");
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error connecting to RabbitMQ: {ex.Message}");
			}

			return result;
		}
	}
}
