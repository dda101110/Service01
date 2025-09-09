using ConsoleApp.CreateRequests;

Console.WriteLine("Create Request to API Key01");

await new RequestModel()
	.UseKey02()
	.SendAsync();

Console.WriteLine("Press any key...");
Console.ReadLine();