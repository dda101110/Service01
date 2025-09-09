using Service01.Models.Models;

namespace Service01.Services
{
	public interface IBufferService
	{
		Task<RateResponseModel?> GetRateAsync(RateRequestModel request);
	}
}
