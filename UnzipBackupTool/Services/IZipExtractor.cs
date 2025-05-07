using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO.Compression;

namespace UnzipBackupTool.Services
{
    public interface IZipExtractor
    {
        void ExtractAllZipFiles();
    }

    public class ZipExtractor : IZipExtractor
    {
        private readonly string _zipDirectory;
        private readonly string _outputDirectory;
        private readonly ILogger<ZipExtractor> _logger;

        public ZipExtractor(IConfiguration configuration, ILogger<ZipExtractor> logger)
        {
            _zipDirectory = configuration["BackupSettings:ZipDirectory"];
            _outputDirectory = configuration["BackupSettings:OutputDirectory"];
            _logger = logger;
        }

        public void ExtractAllZipFiles()
        {
            _logger.LogInformation($"Scanning {_zipDirectory} for ZIP files...");

            if (!Directory.Exists(_zipDirectory))
            {
                _logger.LogError($"ZIP directory {_zipDirectory} does not exist.");
                return;
            }

            var zipFiles = Directory.GetFiles(_zipDirectory, "*.zip");

            if (zipFiles.Length == 0)
            {
                _logger.LogInformation("No ZIP files found.");
                return;
            }

            Directory.CreateDirectory(_outputDirectory);

            foreach (var zipFile in zipFiles)
            {
                ExtractZip(zipFile);
            }
        }

        private void ExtractZip(string zipFilePath)
        {
            _logger.LogInformation($"Extracting {zipFilePath}...");

            try
            {
                var destinationPath = Path.Combine(_outputDirectory, Path.GetFileNameWithoutExtension(zipFilePath));
                Directory.CreateDirectory(destinationPath);

                ZipFile.ExtractToDirectory(zipFilePath, destinationPath, overwriteFiles: true);

                _logger.LogInformation($"Successfully extracted {zipFilePath} to {destinationPath}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to extract {zipFilePath}");
            }
        }
    }

}
