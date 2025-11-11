using AlertVault.Db;
using CliWrap;
using CustomEnvironmentConfig;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace AlertVault.Core.Test;

public class Fixture : IDisposable
{
    private readonly string _testDatabaseName = $"alertvault";
    private readonly string _masterConnectionString;
    private readonly string _testConnectionString;
    private readonly DatabaseContext _context;

    public Configuration.Configuration Configuration { get; }

    public Fixture(IMessageSink messageSink)
    {
        _messageSink = messageSink;
        
        RunBatchFile("docker-compose", "down -v");
        RunBatchFile("docker-compose", "up -d test-db redis");
        
        Configuration = ConfigurationParser.Parse<Configuration.Configuration>(fileName: "local.env");
        
        // Master connection string (to create/drop test database)
        _masterConnectionString = $"Host={Configuration.Postgres.Host};Port={Configuration.Postgres.Port};Database=postgres;Username={Configuration.Postgres.User};Password={Configuration.Postgres.Password}";
        
        // Test database connection string
        _testConnectionString = $"Host={Configuration.Postgres.Host};Port={Configuration.Postgres.Port};Database={_testDatabaseName};Username={Configuration.Postgres.User};Password={Configuration.Postgres.Password}";
        
        WaitForDatabase();
        
        CreateTestDatabase();
        
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseNpgsql(_testConnectionString, b => b.MigrationsAssembly("AlertVault.Db"))
            .Options;
            
        var context = new DatabaseContext(options);
        var pendingMigrations = context.Database.GetPendingMigrations();
        _messageSink.OnMessage(new DiagnosticMessage($"Pending migrations: {string.Join(", ", pendingMigrations)}"));
        context.Database.EnsureCreated();
        _context = context;
    }

    private void CreateTestDatabase()
    {
        using var connection = new Npgsql.NpgsqlConnection(_masterConnectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = $"CREATE DATABASE {_testDatabaseName}";
        command.ExecuteNonQuery();
    }

    public DatabaseContext Context => _context;
    public UserRepository UserRepository => new(Context);
    public AlertRepository AlertRepository => new(Context);
    public AlertNotificationQueueRepository AlertNotificationQueueRepository => new(Context);

    public void Dispose()
    {
        _context.Dispose();
        //DropTestDatabase();
    }

    private void DropTestDatabase()
    {
        using var connection = new Npgsql.NpgsqlConnection(_masterConnectionString);
        connection.Open();
        
        // Terminate existing connections
        using var terminateCmd = connection.CreateCommand();
        terminateCmd.CommandText = $@"
            SELECT pg_terminate_backend(pid) 
            FROM pg_stat_activity 
            WHERE datname = '{_testDatabaseName}'";
        terminateCmd.ExecuteNonQuery();
        
        // Drop database
        using var dropCmd = connection.CreateCommand();
        dropCmd.CommandText = $"DROP DATABASE IF EXISTS {_testDatabaseName}";
        dropCmd.ExecuteNonQuery();
    }
    
    private readonly IMessageSink _messageSink;

    private void RunBatchFile(string command, string argument)
    {
        OutputMessage($"Running: {command} {argument}");

        var result = CliWrap.Cli
            .Wrap(command)
            .WithArguments(argument)                
            .WithStandardOutputPipe(PipeTarget.ToDelegate(OutputMessage))
            .WithStandardErrorPipe(PipeTarget.ToDelegate(OutputMessage))
            .WithValidation(CommandResultValidation.None).ExecuteAsync().GetAwaiter().GetResult();

        void OutputMessage(string message)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                _messageSink.OnMessage(new DiagnosticMessage(message));                                        
            }
        }
    }
    
    private void WaitForDatabase()
    {
        var maxAttempts = 30;
        var delayBetweenAttempts = TimeSpan.FromSeconds(1);
    
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                using var connection = new Npgsql.NpgsqlConnection(_masterConnectionString);
                connection.Open();
                connection.Close();
                _messageSink.OnMessage(new DiagnosticMessage($"Database is ready after {attempt} attempt(s)"));
                return;
            }
            catch (Exception ex)
            {
                if (attempt == maxAttempts)
                {
                    throw new Exception($"Database failed to start after {maxAttempts} attempts: {ex}", ex);
                }
            
                _messageSink.OnMessage(new DiagnosticMessage($"Waiting for database... (attempt {attempt}/{maxAttempts})"));
                Thread.Sleep(delayBetweenAttempts);
            }
        }
    }
}