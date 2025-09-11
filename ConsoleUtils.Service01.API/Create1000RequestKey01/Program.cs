using ConsoleApp.CreateRequests;

Console.WriteLine("Start application ,version 1.5");
Console.WriteLine("Create 1000 Requests to API Key01");

await new APIClient()
	.UseKey01()
	.SetCountRequest(1000)
	.SendAsync();

Console.WriteLine("Press any key...");
Console.ReadLine();