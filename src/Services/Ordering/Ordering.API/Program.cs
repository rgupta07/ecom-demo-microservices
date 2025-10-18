using Ordering.API.Extensions;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddApiServices()
	.AddApplicationServices(builder.Configuration)
	.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
	await app.InitialiseDatabaseAsync();
}

app.Run();
