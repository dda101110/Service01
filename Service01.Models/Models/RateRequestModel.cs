namespace Service01.Models.Models
{
	public class RateRequestModel: BrokerRequestModel
	{
		public string Currency1 { get; set; }
		public string Currency2 { get; set; }
		public string Bank { get; set; }
	}
}
