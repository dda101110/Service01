namespace Service01.Models.Models
{
	public class Service01HostOption: Service01Option
	{
		public string Host { get; set; } = "localhost";
		public int Port { get; set; } = 7777;
	}
}
