using Flurl.Http;

namespace ConsoleApp.CreateRequests
{
	public class Request
	{
		private string _url { get; set; }
		public Request(string url)
		{
			_url = url;
		}

		public async Task SendAsync()
		{
			Console.WriteLine($"Send API request {_url}");
			var result = "error";
			int code = -1;
			var body = "*";
			IFlurlResponse state;

			try
			{
				state = await _url
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

			Console.WriteLine($"RESPONSE from {_url}: {result}");
		}
	}
}
