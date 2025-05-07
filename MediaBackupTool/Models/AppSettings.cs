namespace MediaBackupTool.Models;

public class BackupSettings
{
    public string MediaFolderPath { get; set; } = null!;
    public string BackupOutputPath { get; set; } = null!;
}

public class AppSettings
{
    public BackupSettings BackupSettings { get; set; } = null!;
    public FtpSettings FtpSettings { get; set; } = null!;
}
