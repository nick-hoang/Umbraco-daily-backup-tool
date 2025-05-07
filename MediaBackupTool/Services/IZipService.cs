using System.IO.Compression;

namespace MediaBackupTool.Services;

public interface IZipService
{
    string CreateZip(IEnumerable<string> files, string outputFolder, string sourceFolder);
}

public class ZipService : IZipService
{
    public string CreateZip(IEnumerable<string> files, string outputFolder, string sourceFolder)
    {
        string zipName = Path.Combine(outputFolder, $"Backup_{DateTime.Now:yyyyMMddHHmmss}.zip");

        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        using var zip = ZipFile.Open(zipName, ZipArchiveMode.Create);
        foreach (var file in files)
        {
            // Get the relative path of the file, keep the directory structure
            var entryName = Path.GetRelativePath(sourceFolder ?? string.Empty, file);
            zip.CreateEntryFromFile(file, entryName);
        }

        return zipName;
    }
}
