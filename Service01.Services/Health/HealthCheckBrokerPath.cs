using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Service01.Models.Mnemonics;
using Service01.Models.Models;

namespace Service01.Services.Health
{
	public class HealthCheckBrokerPath : IHealthCheck
	{
		private Service01Option _bufferOption { get; set; }

		public HealthCheckBrokerPath(IOptions<Service01Option> bufferOption)
		{
			_bufferOption = bufferOption.Value;
		}
		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
		{
			string brokerPath = _bufferOption.BrokerPath;
			bool isHealthy = true;
			string resultStateMessage = Mnemonic.HEALTH_CHECK_RESULT_HEALTHY;

			if (string.IsNullOrWhiteSpace(brokerPath))
			{
				isHealthy = false;
				resultStateMessage = Mnemonic.HEALTH_CHECK_RESULT_BROKER_PATH_EMPTY;
			} else if (!Directory.Exists(brokerPath))
			{
				isHealthy = false;
				resultStateMessage = Mnemonic.HEALTH_CHECK_RESULT_BROKER_PATH_INVALID + $" ({brokerPath})";
			}

			if (isHealthy)
			{
				return await Task.FromResult(HealthCheckResult.Healthy(resultStateMessage));
			}
			else
			{
				return await Task.FromResult(HealthCheckResult.Unhealthy(resultStateMessage));
			}
		}
	}
}
