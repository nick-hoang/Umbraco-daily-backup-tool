namespace MediaBackupTool.Models;

public class FtpSettings
{
    public string Host { get; set; } = null!;
    public int Port { get; set; } = 21;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string RemoteFolder { get; set; } = "/";
}
