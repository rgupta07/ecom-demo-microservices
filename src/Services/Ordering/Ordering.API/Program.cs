using BuildingBlocks.Exceptions.Handler;
using Ordering.API.Extensions;
using Ordering.API.Filters;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddApiServices(builder.Configuration)
	.AddApplicationServices(builder.Configuration)
	.AddInfrastructureServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SchemaFilter<EnumSchemaFilter>();
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();


if(app.Environment.IsDevelopment())
{
	await app.InitialiseDatabaseAsync();
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseApiServices();

app.Run();
