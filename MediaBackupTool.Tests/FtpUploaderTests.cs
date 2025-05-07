using MediaBackupTool.Models;
using MediaBackupTool.Services;
using Moq;

public class FtpUploaderTests
{
    [Fact]
    public async Task UploadAsync_UploadsFileSuccessfully()
    {
        // Arrange
        var ftpSettings = new FtpSettings
        {
            Host = "test",
            Port = 21,
            Username = "user",
            Password = "pass",
            RemoteFolder = "/uploads"
        };

        var ftpClientMock = new Mock<IFtpClientWrapper>();

        ftpClientMock.Setup(x => x.ConnectAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        ftpClientMock.Setup(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        ftpClientMock.Setup(x => x.DisconnectAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var uploader = new FtpUploader(ftpClientMock.Object, ftpSettings);

        var testFilePath = "testfile.txt";
        await File.WriteAllTextAsync(testFilePath, "Content");

        // Act
        await uploader.UploadAsync(testFilePath);

        // Assert
        ftpClientMock.Verify(x => x.ConnectAsync(It.IsAny<CancellationToken>()), Times.Once);
        ftpClientMock.Verify(x => x.UploadFileAsync(testFilePath, "/uploads/testfile.txt", It.IsAny<CancellationToken>()), Times.Once);
        ftpClientMock.Verify(x => x.DisconnectAsync(It.IsAny<CancellationToken>()), Times.Once);

        // Cleanup
        File.Delete(testFilePath);
    }
}
