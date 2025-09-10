using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Prometheus;
using Service01.API;
using Service01.Features.Rate.Queries;
using Service01.Models.Models;
using Service01.Services;
using Service01.Services.BrokerService;
using Service01.Services.BufferService;
using Service01.Services.Health;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterMapsterConfiguration();
builder.Services.Configure<Service01Option>(builder.Configuration.GetSection("Service01"));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IBrokerService, FSBrokerService>();
builder.Services.AddScoped<IBufferService, BufferService>();
builder.Services.AddScoped<IValidateService, ValidateService>();

builder.Services.AddHealthChecks()
	.AddCheck<HealthCheckBrokerPath>("HealthCheckBrokerPath")
	.AddCheck<HealthCheckMemory>("HealthCheckMemory")
	.ForwardToPrometheus();
builder.Services.AddHealthChecksUI(opt =>
	{
		opt.SetEvaluationTimeInSeconds(30);
		opt.MaximumHistoryEntriesPerEndpoint(60);
		opt.AddHealthCheckEndpoint(name: "Service01", uri: "/hc");
	})
	.AddInMemoryStorage();

builder.Services.AddControllers();
builder.Services.AddApiVersioning(config =>
{
	config.ApiVersionReader = new HeaderApiVersionReader("api-version");
	config.DefaultApiVersion = new ApiVersion(1, 0);
	config.AssumeDefaultVersionWhenUnspecified = true;
});
builder.Services.AddMediatR(cfg =>
	cfg.RegisterServicesFromAssemblies([
		Assembly.GetExecutingAssembly(), 
		typeof(GetRateQueryHandler).Assembly,
		]));

var app = builder.Build();

app.UseHealthChecksUI(config =>
{
	config.UseRelativeApiPath = false;
	config.UseRelativeResourcesPath = false;
	config.AsideMenuOpened = false;

	config.UIPath = "/healthchecks-ui";
	config.ApiPath = "/healthAPI";
});

app.UseRouting();
app.UseAuthorization();
app.MapMetrics();
app.MapHealthChecks("/health");
app.MapHealthChecks("/hc", new HealthCheckOptions
{
	Predicate = _ => true,
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});

app.MapControllers();

app.Run();
