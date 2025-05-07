namespace MediaBackupTool.Services;

public interface IMediaScanner
{
    IEnumerable<string> GetNewFiles(string mediaFolder, DateTime since);
}

public class MediaScanner : IMediaScanner
{
    public IEnumerable<string> GetNewFiles(string mediaFolder, DateTime since)
    {
        return Directory.EnumerateFiles(mediaFolder, "*.*", SearchOption.AllDirectories)
            .Where(f => File.GetCreationTime(f) >= since);
    }
}
