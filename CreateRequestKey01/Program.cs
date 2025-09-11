using Console.Service01.Host.Shared;
using ConsoleApp.CreateRequests;

System.Console.WriteLine("Start application ,version 1.5");
System.Console.WriteLine("Create ONE Request to API Key01");

await new APIClient()
	.UseKey01()
	.SendAsync();

Pause.PreeAnyKey();