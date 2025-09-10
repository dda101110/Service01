namespace Service01.Models.Models
{
	public class Service01Option
	{
		public string BrokerPath { get; set; }
		public int Timeout { get; set; }
		public int TimeRequestBucket { get; set; }
		public bool SendResponseAfterFillRequestBucket { get; set; }
	}
}
