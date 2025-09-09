using Service01.Models.Models;

namespace Service01.Services
{
	public interface IBrokerService
	{
		Task<RateResponseModel> GetRateAsync(RateRequestModel request);
	}
}
