using Service01.Host;
using Service01.Models.Models;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<Service01HostOption>(builder.Configuration.GetSection("Service01"));
builder.Services.AddHostedService<Service01Host>();

var host = builder.Build();
host.Run();
