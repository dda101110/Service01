using ConsoleApp.CreateRequests;

Console.WriteLine("Start application ,version 1.7");
Console.WriteLine("Create Requests to API Key01 from Plan01");

await new RequestModel()
	.UseKey01()
	.SetCountRequest(5)
	.SendAsync();

await new RequestModel()
	.UseKey01()
	.UseBank("ru01")
	.DisableIndexBank()
	.SetCountRequest(1)
	.SendAsync();

await Task.Delay(1000);

await new RequestModel()
	.UseKey01()
	.UseBank("ru01")
	.DisableIndexBank()
	.SetCountRequest(1)
	.SendAsync();

Console.WriteLine("Press any key...");
Console.ReadLine();