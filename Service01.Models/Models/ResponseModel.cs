namespace Service01.Models.Models
{
	public class ResponseModel : ISimpleErrorResponse
	{
		public string? Item { get; set; }
		public bool Success { get; set; }
		public string? ErrorText { get; set; }
		public int StatusCode { get; set; }
	}
}
