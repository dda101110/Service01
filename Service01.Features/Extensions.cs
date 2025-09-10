using Service01.Features.Rate.Queries;
using Service01.Models.Models;

namespace Service01.Features
{
	public static class Extensions
	{
		public static GetRateQuery? ToGetRateQuery(this string str)
		{
			GetRateQuery result = null;
			string method = string.Empty;
			string path = string.Empty;
			string currency1 = string.Empty;
			string currency2 = string.Empty;
			string bank = string.Empty;

			string[] it1 = str.Split(" ");

			if (it1.Length > 1) {
				method = it1[0];
				path = it1[1];

				it1=path.Split("/").Reverse().ToArray();

				if (it1.Length > 6)
				{
					currency1 = it1[2];
					currency2 = it1[1];
					bank = it1[0];

					result = new GetRateQuery()
					{
						Cur1 = currency1,
						Cur2 = currency2,
						Bank = bank,
						Method = method,
						Path = path,
					};
				}
			}

			return result;
		}

		public static string ToResponseMessage(this RateResponseModel model) {
			var result = $"{model.StatusCode} {model.Item}";

			return result;
		}
	}
}
