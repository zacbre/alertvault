using System.Text.Json.Serialization;
using AlertVault.Core.Configuration;
using AlertVault.Core.Repository;
using AlertVault.Core.Service;
using AlertVault.Core.Validators;
using AlertVault.Db;
using AlertVault.Filters;
using CustomEnvironmentConfig;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var config = ConfigurationParser.Parse<Configuration>(fileName: "local.env");

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
var connectionString = $"Host={config.Postgres.Host};Port={config.Postgres.Port};Database={config.Postgres.Database};Username={config.Postgres.User};Password={config.Postgres.Password}";
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(connectionString));

// repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();

// serivces
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<AlertService>();

// Validators
builder.Services.AddValidatorsFromAssemblyContaining<AlertValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();
app.UseStaticFiles();
app.UseCors(myAllowSpecificOrigins);

app.Run();

public partial class Program { }