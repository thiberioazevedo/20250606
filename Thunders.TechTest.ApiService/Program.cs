using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService;
using Thunders.TechTest.ApiService.Configurations;
using Thunders.TechTest.ApiService.Data;
using Thunders.TechTest.ApiService.Data.Configurations;
using Thunders.TechTest.ApiService.Events;
using Thunders.TechTest.ApiService.Infrastructure.Mappings.Thunders.TechTest.Application.Mappings;
using Thunders.TechTest.ApiService.Interfaces;
using Thunders.TechTest.ApiService.Middlewares;
using Thunders.TechTest.ApiService.Services;
using Thunders.TechTest.OutOfBox.Database;
using Thunders.TechTest.OutOfBox.Queues;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddControllers();
builder.Services.AddCustomSwagger();

var features = Features.BindFromConfiguration(builder.Configuration);

// Add services to the container.
builder.Services.AddProblemDetails();

if (features.UseMessageBroker)
{
    var subscriptionBuilder = new SubscriptionBuilder().Add<PedagioUtilizacaoCriadoEvent>();

    builder.Services.AddBus(builder.Configuration, subscriptionBuilder);
}

if (features.UseEntityFramework)
{
    builder.Services.AddSqlServerDbContext<ApplicationDbContext>(builder.Configuration);
}

//builder.Services.AddAutoMapper(typeof(PedagioUtilizacaoProfile));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddApplicationServices();

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheProvider, CacheProvider>(); 
builder.Services.AddScoped<IEntityCacheProvider, EntityCacheProvider>();

builder.WebHost.UseUrls();

var app = builder.Build();

app.UseCustomSwaggerUI();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.UseValidationExceptionMiddleware();

app.MapDefaultEndpoints();

app.MapControllers();

ApplyMigrations(app);

app.Run();

void ApplyMigrations(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var pendingMigrations = dbContext.Database.GetPendingMigrations();
        if (pendingMigrations.Any())
        {
            Console.WriteLine("Applying pending migrations...");
            dbContext.Database.Migrate();
            Console.WriteLine("Migrations applied successfully.");
        }
        else
        {
            Console.WriteLine("No pending migrations found.");
        }
    }
}