using System.Net.Sockets;
using System.Text;

namespace Console.Service01.Host.Shared
{
	public class HostClient
	{
		private string _host;
		private int _port;

		public HostClient UseHost(string host)
		{
			_host = host;

			return this;
		}
		public HostClient UsePort(int port)
		{
			_port = port;

			return this;
		}
		public async Task SendAsync(string message)
		{
			try
			{
				using TcpClient client = new TcpClient(_host, _port);
				
				System.Console.WriteLine($"Connected to server at {_host}:{_port}");

				NetworkStream stream = client.GetStream();
				using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
				using var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true) { AutoFlush = true };

				await writer.WriteLineAsync(message);

				System.Console.WriteLine($"Sent: {message}");

				string? receivedMessage = await reader.ReadLineAsync();

				System.Console.WriteLine($"Received: {receivedMessage}");
			}
			catch (Exception ex)
			{
				System.Console.WriteLine($"Error: {ex.Message}");
			}
		}
	}
}
