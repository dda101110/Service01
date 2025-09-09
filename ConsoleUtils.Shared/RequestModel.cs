using Flurl.Http;

namespace ConsoleApp.CreateRequests
{
	public class RequestModel
	{
		private string _url { get; set; } = "http://localhost:5019/api/rate/{key}/{bank}";
		private string _key { get; set; } = "";
		private string _bank { get; set; } = "000";

		public RequestModel()
		{
			UseKey01();
		}

		public async Task SendAsync()
		{
			var url = _url
				.Replace("{key}",_key)
				.Replace("{bank}", _bank);

			Console.WriteLine($"Send API request {url}");

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
				result = $"Exception: [{ex.StatusCode}] [{body}]";
			}

			Console.WriteLine($"RESPONSE from {url}: {result}");
		}
		public RequestModel UseKey01()
		{
			_key = "USD/EUR";

			return this;
		}
		public RequestModel UseKey02()
		{
			_key = "CAD/RUB";

			return this;
		}
		public RequestModel UseBank(string bank)
		{
			_bank = bank;

			return this;
		}
	}
}
