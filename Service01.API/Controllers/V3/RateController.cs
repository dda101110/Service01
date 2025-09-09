using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service01.Features.Rate.Queries;

namespace Service01.API.Controllers.V3
{
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiController]
	[ApiVersion("3.0")]
	public class RateController : ControllerBase
	{
		private readonly IMediator _mediator;

		public RateController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		[Route("{currency1}/{currency2}/{bank?}")]
		public async Task<IActionResult> GetRateAsync(string currency1, string currency2, string? bank)
		{
			var request = new GetRateQuery()
			{
				Currency1 = currency1,
				Currency2 = currency2,
				Bank = bank ?? "",
				HttpMethod = Request.Method,
				HttpPath = Request.Path,
			};

			var response = await _mediator.Send(request);

			var result = new ObjectResult(response.Item)
			{
				StatusCode = response.StatusCode,
			};

			return result;
		}
	}
}
