using Mapster;
using MediatR;
using Service01.Models.Models;
using Service01.Services;

namespace Service01.Features.Rate.Queries
{
	public class GetRateQueryHandler : IRequestHandler<GetRateQuery, RateResponseModel>
	{
		private IBrokerService _brokerService;

		public GetRateQueryHandler(IBrokerService brokerService)
		{
			_brokerService = brokerService;
		}
		public async Task<RateResponseModel> Handle(GetRateQuery request, CancellationToken cancellationToken)
		{
			RateRequestModel serviceRequest = request.Adapt<RateRequestModel>();

			var result = await _brokerService.GetRateAsync(serviceRequest);

			return result;
		}
	}
}
