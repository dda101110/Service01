using MediatR;
using Service01.Models.Models;

namespace Service01.Features.Rate.Queries
{
	public class GetRateQuery : IRequest<RateResponseModel>
	{
		public string Cur1 { get; set; }
		public string Cur2 { get; set; }
		public string Bank { get; set; }
		public string Method { get; set; }
		public string Path { get; set; }
	}
}
