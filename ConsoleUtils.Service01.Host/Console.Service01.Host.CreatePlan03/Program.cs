using Console.Service01.Host.Shared;
using ConsoleApp.CreateResponse;

System.Console.WriteLine("Start application ,version 2.2");

string path = "d:\\broker";
string message = "GET http://service01.matchete.ru/api/rate/USD/EUR/TBC";
var countRequest = 15000;
var pauseSec = 20;

HostClient client = new();
await client.UseHost("127.0.0.1")
	.UsePort(7777)
	.SetCountRequest(countRequest)
	.SendAsync(message);

Pause.Delay(pauseSec);

foreach (var i in Enumerable.Range(1,2))
{
	Pause.Delay(pauseSec);
	FSBrokerResponse.AllFromPath(path);
}

Pause.PreeAnyKey();