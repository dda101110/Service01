using MediatR;
using Service01.Models.Models;

namespace Service01.Features.Rate.Queries
{
	public class GetRateQuery : IRequest<RateResponseModel>
	{
		public string Currency1 { get; set; }
		public string Currency2 { get; set; }
		public string Bank { get; set; }
		public string HttpMethod { get; set; }
		public string HttpPath { get; set; }
	}
}
