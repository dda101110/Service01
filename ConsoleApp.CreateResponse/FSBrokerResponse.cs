namespace ConsoleApp.CreateResponse
{
	public class FSBrokerResponse
	{
		private string _file { get; set; }
		private int _delay { get; set; } = 300;
		
		public void Make()
		{
			Thread.Sleep(_delay);
			var filename = Path.GetFileName(_file);
			var ext = Path.GetExtension(_file);
			var name = Path.GetFileNameWithoutExtension(filename);
			var fpath = Path.GetDirectoryName(_file);
			var filenameResponse = $"{fpath}\\{name}.resp";

			if (ext == ".req" && !File.Exists(filenameResponse))
			{
				var code = 200 + DateTime.Now.Second;
				File.WriteAllText(filenameResponse, $"200\r\nRESPONSE {code} {Guid.NewGuid()} {DateTime.Now}");

				Console.WriteLine($"Make response: {name} {DateTime.Now}");
			}
		}
		public FSBrokerResponse UseFile(string file)
		{
			_file = file;

			return this;
		}
		public FSBrokerResponse UseDelay(int delay)
		{
			_delay = delay;

			return this;
		}
		public static void AllFromPath(string path)
		{
			var list = Directory.GetFiles(path);

			Console.WriteLine($"Create {list.Count()} responses");

			foreach (var file in list)
			{
				new FSBrokerResponse()
					.UseFile(file)
					.UseDelay(1)
					.Make();
			}
		}
	}
}
