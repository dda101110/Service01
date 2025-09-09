using ConsoleApp.CreateRequests;

var url = "http://localhost:5019/api/rate/usd/eur";
var urlKestrel = "http://localhost:5000/api/rate/usd/eur";

var tasks = new List<Task>();

for (var i = 1; i <= 10; i++)
{
	var requestUrl = $"{url}/{i}";
	var req = new Request(requestUrl);
	var res = req.SendAsync();

	tasks.Add(res);
	Thread.Sleep(1000 * 15);
}

Console.WriteLine("Wait all responses");

Task.WaitAll(tasks.ToArray());

Console.WriteLine("DONE. Press any key....");
Console.ReadLine();