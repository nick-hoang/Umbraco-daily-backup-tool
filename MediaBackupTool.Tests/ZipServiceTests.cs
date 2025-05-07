using MediaBackupTool.Services;
using System.IO.Compression;

public class ZipServiceTests
{
    [Fact]
    public void CreateZip_CreatesZipWithSpecifiedFiles()
    {
        // Arrange
        var zipService = new ZipService();
        var sourceFolder = "TestSourceFolder";
        var outputFolder = "TestZipOutput";
        Directory.CreateDirectory(outputFolder);

        var testFile = Path.Combine(outputFolder, "test.txt");
        File.WriteAllText(testFile, "Content");

        var files = new[] { testFile };

        // Act
        var zipPath = zipService.CreateZip(files, outputFolder, sourceFolder);

        // Assert
        Assert.True(File.Exists(zipPath));
        using var archive = ZipFile.OpenRead(zipPath);
        Assert.Single(archive.Entries);
        Assert.Equal("test.txt", archive.Entries[0].Name);

        Directory.Delete(outputFolder, true);
    }
}
