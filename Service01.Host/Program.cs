using Service01.API;
using Service01.Features.Rate.Queries;
using Service01.Host;
using Service01.Models.Models;
using Service01.Services;
using Service01.Services.BrokerService;
using Service01.Services.BufferService;
using System.Reflection;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<Service01HostOption>(builder.Configuration.GetSection("Service01"));
builder.Services.Configure<Service01Option>(builder.Configuration.GetSection("Service01"));
builder.Services.AddHostedService<Service01Host>();
builder.Services.AddMediatR(cfg =>
	cfg.RegisterServicesFromAssemblies([
		Assembly.GetExecutingAssembly(),
		typeof(GetRateQueryHandler).Assembly,
		]));
builder.Services.AddScoped<IBrokerService, FSBrokerService>();
builder.Services.AddScoped<IBufferService, BufferService>();
builder.Services.AddScoped<IValidateService, ValidateService>();
builder.Services.RegisterMapsterConfiguration();

var host = builder.Build();
host.Run();
