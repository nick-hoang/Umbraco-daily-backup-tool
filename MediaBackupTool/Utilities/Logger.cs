using Serilog;

namespace MediaBackupTool.Utilities;

public static class Logger
{
    public static void Setup()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File(
                path: "Logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30) // Keep logs for 30 days
            .CreateLogger();
    }
}
