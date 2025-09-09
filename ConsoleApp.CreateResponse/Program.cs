using ConsoleApp.CreateResponse;

var path = "d:\\broker";

var list = Directory.GetFiles(path);

foreach (var file in list.Take(5))
{
	new FSBrokerResponse()
		.UseFile(file)
		.UseDelay(300)
		.Make();
}

foreach (var file in list.Skip(5))
{
	new FSBrokerResponse()
		.UseFile(file)
		.UseDelay(1000)
		.Make();
}