using Microsoft.Extensions.Options;
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

		public Service01Host(IOptions<Service01HostOption> option, ILogger<Service01Host> logger)
		{
			var _option = option.Value;
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
					_ = Task.Run(() => HandleClientAsync(client, stoppingToken));
				}
				catch (OperationCanceledException)
				{
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
			using var _ = client;
			using var ns = client.GetStream();
			using var reader = new StreamReader(ns, Encoding.UTF8, leaveOpen: true);
			await using var writer = new StreamWriter(ns, Encoding.UTF8) { AutoFlush = true };

			var remote = client.Client.RemoteEndPoint;
			_logger.LogInformation("Client connected {Remote}", remote);

			while (!ct.IsCancellationRequested)
			{
				string? line;
				try
				{
					line = await reader.ReadLineAsync(ct);
				}
				catch (IOException)
				{
					break; // client disconnected
				}

				if (line == null) break;

				_logger.LogInformation("Received: {Data}", line);
				await writer.WriteLineAsync($"Echo: {line}");
			}

			_logger.LogInformation("Client disconnected {Remote}", remote);
		}
	}
}
