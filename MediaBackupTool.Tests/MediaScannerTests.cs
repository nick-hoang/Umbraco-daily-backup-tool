using MediaBackupTool.Services;

public class MediaScannerTests
{
    [Fact]
    public void GetNewFiles_ReturnsFilesCreatedSinceYesterday()
    {
        // Arrange
        var scanner = new MediaScanner();
        var testFolder = "TestMedia";
        Directory.CreateDirectory(testFolder);

        var recentFile = Path.Combine(testFolder, "recent.txt");
        File.WriteAllText(recentFile, "Recent");
        File.SetCreationTime(recentFile, DateTime.Now.AddHours(-12));

        var oldFile = Path.Combine(testFolder, "old.txt");
        File.WriteAllText(oldFile, "Old");
        File.SetCreationTime(oldFile, DateTime.Now.AddDays(-2));

        // Act
        var files = scanner.GetNewFiles(testFolder, DateTime.Now.AddDays(-1)).ToList();

        // Assert
        Assert.Single(files);
        Assert.Contains(recentFile, files);

        Directory.Delete(testFolder, true);
    }
}
