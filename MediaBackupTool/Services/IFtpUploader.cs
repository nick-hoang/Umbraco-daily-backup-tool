using FluentFTP;
using MediaBackupTool.Models;
using Polly;
using Serilog;

namespace MediaBackupTool.Services;

public interface IFtpUploader
{
    Task UploadAsync(string filePath, CancellationToken token = default);
}

public class FtpUploader : IFtpUploader
{
    private readonly IFtpClientWrapper _ftpClientWrapper;
    private readonly FtpSettings _ftpSettings;

    public FtpUploader(IFtpClientWrapper ftpClientWrapper, FtpSettings ftpSettings)
    {
        _ftpClientWrapper = ftpClientWrapper;
        _ftpSettings = ftpSettings;
    }
    public async Task UploadAsync(string filePath, CancellationToken token = default)
    {
        var policy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, timeSpan, retryCount, context) =>
                {
                    Log.Warning(ex, "Attempt {RetryCount}: Error uploading. Retrying in {TimeSpan}", retryCount, timeSpan);
                });

        await policy.ExecuteAsync(async () =>
        {
            await _ftpClientWrapper.ConnectAsync(token);
            await _ftpClientWrapper.UploadFileAsync(filePath, $"{_ftpSettings.RemoteFolder}/{Path.GetFileName(filePath)}", token);
            await _ftpClientWrapper.DisconnectAsync(token);
        });
    }
}

