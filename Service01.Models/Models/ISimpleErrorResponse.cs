namespace Service01.Models.Models
{
	public interface ISimpleErrorResponse
	{
		bool Success { get; set; }
		string ErrorText { get; set; }
	}
}
