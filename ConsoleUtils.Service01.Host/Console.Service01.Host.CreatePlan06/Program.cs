using Console.Service01.Host.Shared;
using ConsoleApp.CreateResponse;

System.Console.WriteLine("Start application ,version 2.2");

var path = "d:\\broker";
string message = "GET http://service01.matchete.ru/api/rate/USD/EUR/TBC";

HostClient client = new();

await client.UseHost("127.0.0.1")
	.UsePort(7777)
	.SetStartIndex(1)
	.SetCountRequest(5)
	.SendAsync(message);

client = new();
await client.UseHost("127.0.0.1")
	.UsePort(7777)
	.SetStartIndex(101)
	.SetCountRequest(5)
	.SendAsync(message);

Pause.Delay(20);

client = new();
await client.UseHost("127.0.0.1")
	.UsePort(7777)
	.SetStartIndex(1)
	.SetCountRequest(3)
	.SendAsync(message);

Pause.Delay(15);

await client.UseHost("127.0.0.1")
	.UsePort(7777)
	.SetStartIndex(201)
	.SetCountRequest(5)
	.SendAsync(message);

Pause.Delay(2);

FSBrokerResponse.AllFromPath(path);

System.Console.WriteLine("Press any key...");
System.Console.ReadLine();