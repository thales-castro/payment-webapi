using PaymentWebApi.Database;
using PaymentWebApi.Database.ConnectionStringBuilder;
using PaymentWebApi.Database.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IConnectionStringBuilderService, ConnectionStringBuilderService>();
builder.Services.AddDatabase();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(builder=> { builder.AddConsole(); });

builder.WebHost.UseUrls("http://*:5228");

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddDefaultMongoData(builder.Services);

app.UseAuthorization();

app.MapControllers();

app.Run();
