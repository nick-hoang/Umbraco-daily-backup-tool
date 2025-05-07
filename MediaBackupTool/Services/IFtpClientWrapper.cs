using FluentFTP;
using System.Net;

namespace MediaBackupTool.Services;

public interface IFtpClientWrapper
{
    Task ConnectAsync(CancellationToken token);
    Task UploadFileAsync(string localPath, string remotePath, CancellationToken token);
    Task DisconnectAsync(CancellationToken token);
}

public class FtpClientWrapper : IFtpClientWrapper
{
    private readonly AsyncFtpClient _client;

    public FtpClientWrapper(string host, int port, string username, string password)
    {
        _client = new AsyncFtpClient
        {
            Host = host,
            Port = port,
            Credentials = new NetworkCredential(username, password)
        };
    }

    public async Task ConnectAsync(CancellationToken token)
        => await _client.Connect(token);

    public async Task UploadFileAsync(string localPath, string remotePath, CancellationToken token)
        => await _client.UploadFile(localPath, remotePath, token: token);

    public async Task DisconnectAsync(CancellationToken token)
        => await _client.Disconnect(token);
}
