using Mapster;
using Service01.Features.Rate.Queries;
using Service01.Models.Models;

namespace Service01.API
{
	public static class MapsterConfig
	{
		public static void RegisterMapsterConfiguration(this IServiceCollection services)
		{
			TypeAdapterConfig<GetRateQuery, RateRequestModel>
				.NewConfig()
				.Map(dest => dest.Currency1, src => src.Cur1)
				.Map(dest => dest.Currency2, src => src.Cur2)
				.Map(dest => dest.HttpMethod, src => src.Method)
				.Map(dest => dest.HttpPath, src => src.Path);
		}
	}
}
