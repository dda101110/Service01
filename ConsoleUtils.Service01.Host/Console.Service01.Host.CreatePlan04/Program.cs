using Console.Service01.Host.Shared;
using ConsoleApp.CreateResponse;

System.Console.WriteLine("Start application ,version 2.0");

string path = "d:\\broker";
string message = "GET http://service01.matchete.ru/api/rate/USD/EUR/TBC";

HostClient client = new();
await client.UseHost("127.0.0.1")
	.UsePort(7777)
	.SetCountRequest(2)
	.SendAsync(message);

Pause.Delay(60);

client = new();
await client.UseHost("127.0.0.1")
	.UsePort(7777)
	.SendAsync(message);

Pause.Delay(30);

FSBrokerResponse.AllFromPath(path);

Pause.PreeAnyKey();