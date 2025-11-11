using System.Text.Json.Serialization;
using AlertVault.Core.Configuration;
using AlertVault.Core.Jobs;
using AlertVault.Core.Repository;
using AlertVault.Core.Service;
using AlertVault.Core.Validators;
using AlertVault.Db;
using AlertVault.Filters;
using CustomEnvironmentConfig;
using FluentValidation;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var config = ConfigurationParser.Parse<Configuration>(fileName: "local.env");

Redis = ConnectionMultiplexer.Connect(config.Redis.ToString());

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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<IAlertNotificationQueueRepository, AlertNotificationQueueRepository>();

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
    .UseRedisStorage(config.Redis.ToString()));
builder.Services.AddHangfireServer();

builder.Services.AddTransient<IJobInstance, FindExpiredAlertsJob>();
builder.Services.AddTransient<IJobInstance, NotifyExpiredAlertJob>();

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

RecurringJob.AddOrUpdate<IJobInstance>("ijob-instances", x => x.Execute(), Cron.Minutely);

app.Run();

public partial class Program
{
    public static ConnectionMultiplexer? Redis;
}