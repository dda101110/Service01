using ConsoleApp.CreateRequests;
using ConsoleApp.CreateResponse;

var countRequest = 1000;
var wait = 20;

Console.WriteLine("Start application ,version 1.11");
Console.WriteLine($"Create Requests to API Key01 from Plan01");
Console.WriteLine($"\r\nCreate {countRequest} requests");

await new APIClient()
	.UseKey01()
	.SetCountRequest(countRequest)
	.SendAsync();

System.Console.WriteLine($"Pause {wait} sec...");

Thread.Sleep(1000 * wait);

var path = "d:\\broker";
var list = Directory.GetFiles(path);

Console.WriteLine($"Create {list.Count()} responses");

foreach (var file in list)
{
	new FSBrokerResponse()
		.UseFile(file)
		.UseDelay(50)
		.Make();
}

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

Console.WriteLine("Press any key...");
Console.ReadLine();