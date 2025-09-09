using System.Collections.Concurrent;

namespace Service01.Models.Models
{
	public class BufferItemState
	{
		public DateTime DateTimeEnd { get; set; }
		public RateResponseModel? Item { get; set; }
		public ConcurrentQueue<SemaphoreSlim> Semaphores { get; set; } = new ConcurrentQueue<SemaphoreSlim>();
	}
}
