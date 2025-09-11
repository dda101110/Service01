using Console.Service01.Host.Shared;
using ConsoleApp.CreateRequests;
using ConsoleApp.CreateResponse;

var path = "d:\\broker";
var countRequest = 1000;
var wait = 20;

System.Console.WriteLine("Start application ,version 1.12");
System.Console.WriteLine($"Create Requests to API Key01 from Plan01");
System.Console.WriteLine($"\r\nCreate {countRequest} requests");

await new APIClient()
	.UseKey01()
	.SetCountRequest(countRequest)
	.SendAsync();

Pause.Delay(15);

FSBrokerResponse.AllFromPath(path);

//await new RequestModel()
//	.UseKey01()
//	.UseBank("ru01")
//	.DisableIndexBank()
//	.SetCountRequest(1)
//	.SendAsync();

//await Task.Delay(1000);

//await new RequestModel()
//	.UseKey01()
//	.UseBank("ru01")
//	.DisableIndexBank()
//	.SetCountRequest(1)
//	.SendAsync();

Pause.PreeAnyKey();