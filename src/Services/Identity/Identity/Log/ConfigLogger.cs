using Serilog;
namespace Identity.Log;

public static class ConfigLogger
{
    public static LoggerConfiguration GetLoggerConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
            .AddEnvironmentVariables()
            .Build();

        return new LoggerConfiguration().ReadFrom.Configuration(configuration);
    }
}