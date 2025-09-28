

using BuildingBlocks.Exceptions.Handler;
using Discount.gRPC.Utilities.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DiscountDbContext>(opts =>
{
	opts.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"));
});

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.MigrateDatabase();

app.MapGrpcService<DiscountService>();

// Configure the HTTP request pipeline.
app.UseExceptionHandler(opts => { });

app.Run();
