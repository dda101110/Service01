namespace Console.Service01.Host.Shared
{
	public static class Pause
	{
		public static void Delay(int sec)
		{
			System.Console.WriteLine($"Pause {sec} sec");

			foreach(var t in Enumerable.Range(1, sec))
			{
				System.Console.Write(".");
				Thread.Sleep(1000);
			}

			System.Console.WriteLine($"done {sec} sec");
		}
		public static void PreeAnyKey()
		{
			System.Console.WriteLine("Press any key...");
			System.Console.ReadLine();
		}
	}
}
