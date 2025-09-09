using Microsoft.AspNetCore.Mvc;
using Service01.Models.Models;
using Service01.Services;

namespace Service01.API.Controllers
{
	[Route("api/[controller]")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiController]
	[ApiVersion("1.0")]
	public class RateController : ControllerBase
	{
		private IBufferService _bufferService {  get; set; }
		private IBrokerService _brokerService { get; set; }

		public RateController(IBufferService bufferService, IBrokerService brokerService)
		{
			_bufferService = bufferService;
			_brokerService = brokerService;
		}

		[HttpGet]
		[Route("{currency1}/{currency2}/{bank?}")]
		public async Task<IActionResult> GetRateAsync(string currency1, string currency2, string? bank) {
			var request = new RateRequestModel()
			{
				Currency1 = currency1,
				Currency2 = currency2,
				Bank = bank ?? "",
				HttpMethod = Request.Method,
				HttpPath = Request.Path,
			};

			var response = await _brokerService.GetRateAsync(request);

			var result = new ObjectResult(response.Item) 
			{ 
				StatusCode = response.StatusCode,
			};

			return result;
		}
	}
}
