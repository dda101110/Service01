using Flurl.Http;

namespace ConsoleApp.CreateRequests
{
	public class APIClient
	{
		private string _url { get; set; } = "http://localhost:5019/api/v1/rate/{key}/{bank}";
		private string _key { get; set; } = "";
		private string _bank { get; set; } = "000";
		private int _countRequest { get; set; } = 1;
		private bool _useIndexBank { get; set; } = false;

		public APIClient()
		{
			UseKey01();
			UseIndexBank();
		}

		public async Task SendAsync()
		{
			foreach (var indexRequest in Enumerable.Range(1, _countRequest)) {
				_ = SendOneRequestAsync(indexRequest);
			}
		}
		public async Task SendOneRequestAsync(int indexRequest)
		{
			var url = _url
				.Replace("{key}",_key)
				.Replace("{bank}", _bank + (_useIndexBank ? indexRequest.ToString():""));

			Console.WriteLine($"Send API Request [{indexRequest}] {url}");

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
				result = $"Exception: [{ex.StatusCode}] [{body}] [{ex.Message}]";
			}

			Console.WriteLine($"RESPONSE from {url}: {result}");
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
	}
}
