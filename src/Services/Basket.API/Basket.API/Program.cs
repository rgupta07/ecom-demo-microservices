using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
var configuration = builder.Configuration;

// Add Services to the container
builder.Services.AddCarter();

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddScoped<IShopingCartRepository, ShoppingCartRepository>();

builder.Services.AddMediatR(config =>
{
	config.RegisterServicesFromAssembly(assembly);
	config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
	config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});

builder.Services.AddMarten(builder =>
{
	builder.Connection(configuration.GetConnectionString("BasketDBConnection")!);
	builder.Schema.For<ShoppingCart>().Identity(x => x.UserName);
	builder.AutoCreateSchemaObjects = AutoCreate.All;
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
	builder.Services.InitializeMartenWith<BasketInitialDataSetup>();

builder.Services.AddHealthChecks()
	.AddNpgSql(configuration.GetConnectionString("BasketDBConnection")!,
	name: "PostgreSQL",
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
