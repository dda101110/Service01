using Console.Service01.Host.Shared;
using ConsoleApp.CreateResponse;

System.Console.WriteLine("Start application ,version 2.2");

Pause.Delay(5);							// 5 sec

var path = "d:\\broker";
string messageKey01 = "GET http://service01.matchete.ru/api/rate/USD/EUR/TBC";
string messageKey02 = "GET http://service01.matchete.ru/api/rate/CAD/RUB/TBC";

HostClient client = new();
await client.UseHost("127.0.0.1")
	.UsePort(7777)
	.SetStartIndex(1)
	.SetCountRequest(5)
	.SendAsync(messageKey01);

Pause.Delay(10);						// 15 sec

client = new();
await client.UseHost("127.0.0.1")
	.UsePort(7777)
	.SetStartIndex(101)
	.SetCountRequest(5)
	.SendAsync(messageKey01);

Pause.Delay(25);						// 40 sec

client = new();
await client.UseHost("127.0.0.1")
	.UsePort(7777)
	.SetStartIndex(201)
	.SetCountRequest(5)
	.SendAsync(messageKey01);

Pause.Delay(10);						// 50 sec

client = new();
await client.UseHost("127.0.0.1")
	.UsePort(7777)
	.SetStartIndex(201)
	.SetCountRequest(5)
	.SendAsync(messageKey02);

Pause.Delay(2);							// 52 sec

FSBrokerResponse.AllFromPath(path);

Pause.PreeAnyKey();