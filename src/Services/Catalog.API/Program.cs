using Catalog.API.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using JasperFx;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(config =>
{
	config.RegisterServicesFromAssembly(typeof(Program).Assembly);
	config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
	config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});

builder.Services.AddMarten(opts =>
{
	opts.Connection(builder.Configuration.GetConnectionString("CatalogDBConnection")!);
	opts.AutoCreateSchemaObjects = AutoCreate.All;
});

if(builder.Environment.IsDevelopment())
	builder.Services.InitializeMartenWith<CatalogInitialDataSetup>();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddCarter();

builder.Services.AddHealthChecks()
	.AddNpgSql(builder.Configuration.GetConnectionString("CatalogDBConnection")!, name: "CatalogDB-Check");

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
