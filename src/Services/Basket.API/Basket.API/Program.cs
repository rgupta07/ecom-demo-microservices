using Basket.API.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
var configuration = builder.Configuration;

builder.Services
	.AddApiServices(configuration, assembly)
	.AddMapping();

if (builder.Environment.IsDevelopment())
	builder.Services.InitializeMartenWith<BasketInitialDataSetup>();

builder.Services.AddHealthChecks()
	.AddNpgSql(configuration.GetConnectionString("BasketDBConnection")!,
	name: "PostgreSQLConnection",
	tags: ["ready"])
	.AddRedis(configuration.GetConnectionString("RedisCacheConnection")!,
	name: "RedisConnection",
	tags: ["ready"]);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();


var app = builder.Build();
  
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapCarter();

app.UseExceptionHandler(opts => { });

app.UseHealthChecks("/healthcheck",
	new HealthCheckOptions()
	{
		ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
	});

app.Run();
