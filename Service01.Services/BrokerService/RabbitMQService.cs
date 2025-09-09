using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Service01.Models.Models;
using System.Text;

namespace Service01.Services.BrokerService
{
	public class RabbitMQService : IBrokerService
	{
		private SemaphoreSlim _semaphore {  get; set; } = new SemaphoreSlim(1);
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

			_semaphore.WaitAsync();
		}
		public async Task<RateResponseModel> GetRateAsync(RateRequestModel request)
		{
			var result = new RateResponseModel();
			var mainQueue = "rate";
			var replyQueue = $"reply_queue_for_rate_{Guid.NewGuid}";

			try
			{
				using (var connection = await _factory.CreateConnectionAsync())
				using (var channel = await connection.CreateChannelAsync())
				{
					await channel.QueueDeclareAsync(queue: mainQueue,
										 durable: false,
										 exclusive: false,
										 autoDelete: true,
										 arguments: null);

					var consumer = new AsyncEventingBasicConsumer(channel);

					consumer.ReceivedAsync += async (sender, @event) => 
					{
						var body = @event.Body.ToArray();
						var message = System.Text.Encoding.UTF8.GetString(body);
						
						_logger.LogInformation($"RabbutMQ Received: {message}");

						await channel.BasicAckAsync(deliveryTag: @event.DeliveryTag, multiple: false);

						result.Item = message;
						result.Success = true;

						_semaphore?.Release();
					};

					await channel.BasicConsumeAsync(replyQueue, true, consumer);

					await channel.QueueDeclareAsync(queue: mainQueue,
										 durable: false,
										 exclusive: false,
										 autoDelete: false,
										 arguments: null);

					string message = JsonConvert.SerializeObject(request);
					var body = Encoding.UTF8.GetBytes(message);

					await channel.BasicPublishAsync(exchange: "",
										 routingKey: "key",
										 body: body);

					_logger.LogInformation($"RabbitMQ Sent: {message}");

					await _semaphore.WaitAsync();
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
