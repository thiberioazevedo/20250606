using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService;
using Thunders.TechTest.OutOfBox.Database;
using Thunders.TechTest.OutOfBox.Queues;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddControllers();

var features = Features.BindFromConfiguration(builder.Configuration);

// Add services to the container.
builder.Services.AddProblemDetails();

if (features.UseMessageBroker)
{
    builder.Services.AddBus(builder.Configuration, new SubscriptionBuilder());
}

if (features.UseEntityFramework)
{
    builder.Services.AddSqlServerDbContext<DbContext>(builder.Configuration);
}


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapDefaultEndpoints();

app.MapControllers();

app.Run();
