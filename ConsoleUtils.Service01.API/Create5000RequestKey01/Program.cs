using Console.Service01.Host.Shared;
using ConsoleApp.CreateRequests;

System.Console.WriteLine("Start application ,version 1.5");
System.Console.WriteLine("Create 5000 Requests to API Key01");

await new APIClient()
	.UseKey01()
	.SetCountRequest(5000)
	.SendAsync();

Pause.PreeAnyKey();