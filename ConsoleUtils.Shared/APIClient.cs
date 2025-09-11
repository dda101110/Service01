using Flurl.Http;
using System.Collections.Concurrent;

namespace ConsoleApp.CreateRequests
{
	public class APIClient
	{
		private string _url { get; set; } = "http://localhost:5019/api/v1/rate/{key}/{bank}";
		private string _key { get; set; } = "";
		private string _bank { get; set; } = "000";
		private int _countRequest { get; set; } = 1;
		private bool _useIndexBank { get; set; } = false;
		private int _startIndex { get; set; } = 1;
		private ConcurrentDictionary<int, DateTime> _timeRequests = new();

		public APIClient()
		{
			UseKey01();
			UseIndexBank();
		}

		public async Task SendAsync()
		{
			foreach (var indexRequest in Enumerable.Range(_startIndex, _countRequest)) {
				_ = SendOneRequestAsync(indexRequest);
			}
		}
		public async Task SendOneRequestAsync(int indexRequest)
		{
			var url = _url
				.Replace("{key}",_key)
				.Replace("{bank}", _bank + (_useIndexBank ? indexRequest.ToString():""));
			var dtNow = DateTime.Now;
			_timeRequests.AddOrUpdate(indexRequest, dtNow, (i, o) => dtNow);

			Console.WriteLine($"Sent[{indexRequest:00000}][{dtNow.ToString("HH:mm:ss")}] API Request: {url}");

			var result = "error";
			int code = -1;
			var body = "*";
			IFlurlResponse state;

			try
			{
				state = await url
					.WithTimeout(TimeSpan.FromMinutes(1))
					.GetAsync();

				code = state.StatusCode;
				body = await state.GetStringAsync();
				result = $"[{code}] {body}";
			}
			catch (FlurlHttpException ex)
			{
				result = $"Exception[{indexRequest:00000}]: [{ex.StatusCode}] [{body}] [{ex.Message}]";
			}

			dtNow = DateTime.Now;
			_timeRequests.Remove(indexRequest, out var dtStart);

			Console.WriteLine($"RESPONSE[{indexRequest:00000}][{dtStart.ToString("HH:mm:ss")}-{dtNow.ToString("HH:mm:ss")}][{(dtNow - dtStart).TotalSeconds:000.00} sec]: {result}");
		}
		public APIClient UseKey01()
		{
			_key = "USD/EUR";

			return this;
		}
		public APIClient UseKey02()
		{
			_key = "CAD/RUB";

			return this;
		}
		public APIClient UseBank(string bank)
		{
			_bank = bank;

			return this;
		}
		public APIClient SetCountRequest(int countRequest)
		{
			_countRequest = countRequest;

			return this;
		}
		public APIClient UseIndexBank()
		{
			_useIndexBank = true;

			return this;
		}
		public APIClient DisableIndexBank()
		{
			_useIndexBank = false;

			return this;
		}
		public APIClient SetStartIndex(int startIndex)
		{
			_startIndex = startIndex;

			return this;
		}
	}
}
