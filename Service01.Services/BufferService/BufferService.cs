using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service01.Models.Models;
using System.Collections.Concurrent;

namespace Service01.Services.BufferService
{
	public class BufferService : IBufferService
	{
		private static ConcurrentDictionary<string, List<BufferItemState>> _buffer { get; set; } = new ConcurrentDictionary<string, List<BufferItemState>>();
		private IBrokerService _brokerService { get; set; }
		private Service01Option _bufferOption { get; set; }
		private ILogger<BufferService> _logger { get; set; }
		private SemaphoreSlim _semaphore { get; set; } = new SemaphoreSlim(1);

		public BufferService(IBrokerService brokerService, IOptions<Service01Option> bufferOption, ILogger<BufferService> logger)
		{
			_brokerService = brokerService;
			_bufferOption = bufferOption.Value;
			_logger = logger;

			_semaphore.WaitAsync();
		}

		public async Task<RateResponseModel?> GetRateAsync(RateRequestModel request)
		{
			string keyRequest = GetKey(request);
			var result = new RateResponseModel();

			var dt = DateTime.Now;
			var dateTimeEnd = dt + TimeSpan.FromSeconds(_bufferOption.TimeRequestBucket);
			var newItemState = new BufferItemState()
			{
				DateTimeEnd = dateTimeEnd,
				Item = null,
				Semaphores = new ConcurrentQueue<SemaphoreSlim>(),
			};
			var bufferItemState = _buffer.GetOrAdd(keyRequest,new List<BufferItemState>() {
				newItemState
			});

			var existsItemState = bufferItemState.FirstOrDefault(x => x.DateTimeEnd.Ticks >= dt.Ticks);
			if (existsItemState is null) {
				existsItemState = newItemState;
				bufferItemState.Add(newItemState);
			}

			var isFirstRequest = existsItemState.DateTimeEnd.Ticks == dateTimeEnd.Ticks;
			if (isFirstRequest) {
				_logger.LogDebug($"Make first request to BrokerService from RequestBacket");

				existsItemState.Item = await _brokerService.GetRateAsync(request);

				TimeSpan differenceDelay = dateTimeEnd - DateTime.Now;

				if (differenceDelay > TimeSpan.FromMicroseconds(0) && _bufferOption.SendResponseAfterFillRequestBucket)
				{
					await Task.Delay(differenceDelay);
				}

				foreach (var semaphore in existsItemState.Semaphores)
				{
					semaphore?.Release();
				}

				existsItemState.Semaphores = new ConcurrentQueue<SemaphoreSlim>();
			} else
			{
				if (existsItemState.Item is null)
				{
					existsItemState.Semaphores.Enqueue(_semaphore);

					_logger.LogDebug($"Append request in RequestBacket");

					await _semaphore.WaitAsync();
				}
			}

			result = existsItemState.Item;

			return result;
		}

		private string GetKey(RateRequestModel request)
		{
			string result = request.Currency1 + request.Currency2;

			return result;
		}
	}
}
