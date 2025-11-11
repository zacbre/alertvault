using System.Text.Json.Serialization;
using AlertVault.Core.Configuration;
using AlertVault.Core.Infrastructure.Database;
using AlertVault.Core.Jobs;
using AlertVault.Core.Service;
using AlertVault.Core.Validators;
using AlertVault.Filters;
using CustomEnvironmentConfig;
using FluentValidation;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var config = ConfigurationParser.Parse<Configuration>(fileName: "local.env");

AlertVault.Program.Redis = ConnectionMultiplexer.Connect(config.Redis.ToString());

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy  =>
        {
            policy.WithOrigins(config.Cors)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddSingleton(config);
builder.Services.AddOpenApi();

builder
    .Services
    .AddControllers(options =>
    {
        options.Filters.Add<ResultFilter>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
// build connection string.
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(config.Postgres.ToString()));

// repositories
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<AlertRepository>();
builder.Services.AddScoped<AlertNotificationQueueRepository>();

// serivces
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<AlertService>();
builder.Services.AddTransient<AlertNotificationQueueService>();

// Validators
builder.Services.AddValidatorsFromAssemblyContaining<AlertValidator>();

// Jobs
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseRedisStorage(AlertVault.Program.Redis));

builder.Services.AddHangfireServer();

builder.Services.AddTransient<FindExpiredAlertsJob>();
builder.Services.AddTransient<NotifyExpiredAlertJob>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();
app.UseStaticFiles();
app.UseCors(myAllowSpecificOrigins);

app.UseHangfireDashboard();

RecurringJob.AddOrUpdate<FindExpiredAlertsJob>(
    nameof(FindExpiredAlertsJob), 
    x => x.Execute(), 
    Cron.Minutely);

RecurringJob.AddOrUpdate<NotifyExpiredAlertJob>(
    nameof(NotifyExpiredAlertJob), 
    x => x.Execute(), 
     Cron.Minutely);

app.Run();

namespace AlertVault
{
    public partial class Program
    {
        public static ConnectionMultiplexer? Redis;
    }
}