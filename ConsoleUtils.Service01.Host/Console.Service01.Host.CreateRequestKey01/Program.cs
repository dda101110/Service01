using Console.Service01.Host.Shared;

System.Console.WriteLine("Start application ,version 2.2");

string message = "GET http://service01.matchete.ru/api/rate/USD/EUR/001";

HostClient client = new();

await client.UseHost("127.0.0.1")
	.UsePort(7777)
	.SendAsync(message);

Pause.PreeAnyKey();