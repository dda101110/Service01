using MediatR;
using Microsoft.Extensions.Options;
using Service01.Features;
using Service01.Models.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Service01.Host
{
	public class Service01Host : BackgroundService
	{
		private Service01HostOption _option;
		private readonly ILogger<Service01Host> _logger;
		private readonly TcpListener _listener;
		private readonly IServiceProvider _services;

		public Service01Host(IOptions<Service01HostOption> option, IServiceProvider services, ILogger<Service01Host> logger)
		{
			var _option = option.Value;
			_services = services;
			_listener = new TcpListener(IPAddress.Parse(_option.Host), _option.Port);
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_listener.Start();
			_logger.LogInformation("TCP listener started on {Endpoint}", _listener.LocalEndpoint);

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					var client = await _listener.AcceptTcpClientAsync(stoppingToken);
					_ = HandleClientAsync(client, stoppingToken);
				}
				catch (OperationCanceledException)
				{
					_logger.LogError("OperationCanceled");

					break;
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Accept error");
				}
			}

			_listener.Stop();
			_logger.LogInformation("TCP listener stopped");
		}

		private async Task HandleClientAsync(TcpClient client, CancellationToken ct)
		{
			using var scope = _services.CreateScope();
			using var _ = client;
			using var ns = client.GetStream();
			using var reader = new StreamReader(ns, Encoding.UTF8, leaveOpen: true);
			using var writer = new StreamWriter(ns, Encoding.UTF8, leaveOpen: true) { AutoFlush = true };

			var remote = client.Client.RemoteEndPoint;
			_logger.LogInformation("Client connected {Remote}", remote);

			while (!ct.IsCancellationRequested)
			{
				string? clientRequest;

				try
				{
					clientRequest = await reader.ReadLineAsync(ct);
				}
				catch (IOException ex)
				{
					_logger.LogError("Exception: {Data}", ex.Message);

					break;
				}

				if (clientRequest == null)
				{
					break;
				}

				_logger.LogInformation("Service01.Host Received: {Data}", clientRequest);

				var request = clientRequest.ToGetRateQuery();
				var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

				var response = await mediator.Send(request, ct);

				var responseMessage = response.ToResponseMessage();

				await writer.WriteLineAsync(responseMessage);

				_logger.LogInformation("Service01.Host Response: {response}", responseMessage);
			}

			_logger.LogInformation("Client disconnected {Remote}", remote);
		}
	}
}
