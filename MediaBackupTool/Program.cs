using MediaBackupTool.Models;
using MediaBackupTool.Services;
using MediaBackupTool.Utilities;
using Microsoft.Extensions.Configuration;
using Serilog;

Logger.Setup();

// Load configuration
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var settings = config.Get<AppSettings>();

// Initialize services
IMediaScanner scanner = new MediaScanner();
IZipService zipService = new ZipService();
// Setup FTP client wrapper and uploader
IFtpClientWrapper ftpClientWrapper = new FtpClientWrapper(
    settings.FtpSettings.Host,
    settings.FtpSettings.Port,
    settings.FtpSettings.Username,
    settings.FtpSettings.Password
);

IFtpUploader ftpUploader = new FtpUploader(ftpClientWrapper, settings.FtpSettings);

// Perform backup
try
{
    var files = scanner.GetNewFiles(settings.BackupSettings.MediaFolderPath, DateTime.Now.AddDays(-1)).ToList();

    if (files.Any())
    {
        Log.Information("Found {Count} new files.", files.Count);

        var zipPath = zipService.CreateZip(files, settings.BackupSettings.BackupOutputPath, settings.BackupSettings.MediaFolderPath);
        Log.Information("Created zip archive at {ZipPath}", zipPath);

        await ftpUploader.UploadAsync(zipPath);
        Log.Information("Successfully uploaded backup to FTP.");
    }
    else
    {
        Log.Information("No new files found to backup.");
    }
}
catch (Exception ex)
{
    Log.Error(ex, "Backup operation failed.");
}

Log.CloseAndFlush();
