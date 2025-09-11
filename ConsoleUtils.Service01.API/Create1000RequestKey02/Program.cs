using Console.Service01.Host.Shared;
using ConsoleApp.CreateRequests;

System.Console.WriteLine("Start application ,version 1.6");
System.Console.WriteLine("Create 1000 Requests to API Key02");

await new APIClient()
	.UseKey02()
	.SetCountRequest(1000)
	.SendAsync();

Pause.PreeAnyKey();