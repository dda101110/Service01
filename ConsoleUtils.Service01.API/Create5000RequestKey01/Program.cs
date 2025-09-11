using ConsoleApp.CreateRequests;

Console.WriteLine("Start application ,version 1.5");
Console.WriteLine("Create 5000 Requests to API Key01");

await new APIClient()
	.UseKey01()
	.SetCountRequest(5000)
	.SendAsync();

Console.WriteLine("Press any key...");
Console.ReadLine();