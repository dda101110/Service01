using ConsoleApp.CreateRequests;

Console.WriteLine("Start application ,version 1.5");
Console.WriteLine("Create ONE Request to API Key01");

await new RequestModel()
	.UseKey01()
	.SendAsync();

Console.WriteLine("Press any key...");
Console.ReadLine();