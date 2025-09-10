using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Service01.Models.Mnemonics;
using Service01.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service01.Services.Health
{
	public class HealthCheckMemory : IHealthCheck
	{
		private Service01Option _bufferOption { get; set; }

		public HealthCheckMemory(IOptions<Service01Option> bufferOption)
		{
			_bufferOption = bufferOption.Value;
		}
		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
		{
			bool isHealthy = true;
			string resultStateMessage = Mnemonic.HEALTH_CHECK_RESULT_HEALTHY;

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
