using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;

namespace Console.Service01.Host.Shared
{
	public class HostClient
	{
		private string _host;
		private int _port;
		private int _countRequest { get; set; } = 1;
		private bool _useIndexBank {  get; set; } = false;
		private ConcurrentDictionary<int, DateTime> _timeRequests = new();

		public HostClient()
		{
			UseIndexBank();
		}

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
		public HostClient SetCountRequest(int countRequest)
		{
			_countRequest = countRequest;

			return this;
		}
		public HostClient UseIndexBank()
		{
			_useIndexBank = true;

			return this;
		}
		public HostClient DisableIndexBank()
		{
			_useIndexBank = false;

			return this;
		}
		public async Task SendAsync(string message)
		{
			foreach (var indexRequest in Enumerable.Range(1, _countRequest))
			{
				_ = SendOneRequestAsync(message, indexRequest);
			}
		}
		public async Task SendOneRequestAsync(string message, int indexRequest=1)
		{
			try
			{
				using TcpClient client = new TcpClient(_host, _port);
				
				System.Console.WriteLine($"Connected[{indexRequest}] to server at {_host}:{_port}");

				NetworkStream stream = client.GetStream();
				using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
				using var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true) { AutoFlush = true };

				string sendMessage = string.Empty;

				if (_useIndexBank)
				{
					sendMessage = message + indexRequest.ToString();
				} else
				{
					sendMessage = message;
				}

				var dtNow = DateTime.Now;
				_timeRequests.AddOrUpdate(indexRequest, dtNow, (i,o)=> dtNow);

				await writer.WriteLineAsync(sendMessage);

				System.Console.WriteLine($"Sent[{indexRequest}][{dtNow.ToString("HH:mm:ss")}]: {sendMessage}");

				string? receivedMessage = await reader.ReadLineAsync();

				dtNow = DateTime.Now;
				_timeRequests.Remove(indexRequest, out var dtStart);

				System.Console.WriteLine($"Received[{indexRequest}][{dtStart.ToString("HH:mm:ss")}-{dtNow.ToString("HH:mm:ss")}][{(dtNow-dtStart).TotalSeconds} sec]: {receivedMessage}");
			}
			catch (Exception ex)
			{
				System.Console.WriteLine($"Error[{indexRequest}]: {ex.Message}");
			}
		}
	}
}
