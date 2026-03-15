using ei8.Avatar.Installer.Domain.Model.Plugins;
using Microsoft.Extensions.Logging;
using neurUL.Common.Domain.Model;
using System.IO.Compression;

namespace ei8.Avatar.Installer.IO.Process.Services.Plugins
{
    public class PluginsService : IPluginsService
    {
        private readonly ILogger<PluginsService> logger;

        public PluginsService(ILogger<PluginsService> logger)
        {
            this.logger = logger;
        }

        public async Task DownloadAndExtractAsync(string destinationPath, string url, string subPath)
        {
            AssertionConcern.AssertArgumentNotNull(destinationPath, nameof(destinationPath));
            AssertionConcern.AssertArgumentNotNull(url, nameof(url));

            var tempZipPath = Path.GetTempFileName();
            var tempExtractDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            try
            {
                this.logger.LogInformation("Downloading plugin archive from {url}...", url);

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("AvatarInstaller/1.0");
                    using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();

                    using var fileStream = new FileStream(tempZipPath, FileMode.Create, FileAccess.Write, FileShare.None);
                    await response.Content.CopyToAsync(fileStream);
                }

                this.logger.LogInformation("Extracting archive...");
                ZipFile.ExtractToDirectory(tempZipPath, tempExtractDir);

                // Archives often have a single root folder; detect it
                var rootEntries = Directory.GetDirectories(tempExtractDir);
                var effectiveRoot = rootEntries.Length == 1 ? rootEntries[0] : tempExtractDir;

                var sourcePath = string.IsNullOrWhiteSpace(subPath)
                    ? effectiveRoot
                    : Path.Combine(effectiveRoot, subPath.Replace('/', Path.DirectorySeparatorChar));

                if (!Directory.Exists(sourcePath))
                    throw new DirectoryNotFoundException(
                        $"Path '{subPath}' not found in the downloaded archive."
                    );

                this.logger.LogInformation("Copying plugins to {destinationPath}...", destinationPath);
                PluginsService.CopyDirectory(sourcePath, destinationPath);

                this.logger.LogInformation("Plugins installed successfully to {destinationPath}.", destinationPath);
            }
            finally
            {
                if (File.Exists(tempZipPath))
                    File.Delete(tempZipPath);
                if (Directory.Exists(tempExtractDir))
                    Directory.Delete(tempExtractDir, recursive: true);
            }
        }

        private static void CopyDirectory(string sourceDir, string destinationDir)
        {
            Directory.CreateDirectory(destinationDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                var destFile = Path.Combine(destinationDir, Path.GetFileName(file));
                File.Copy(file, destFile, overwrite: true);
            }

            foreach (var dir in Directory.GetDirectories(sourceDir))
            {
                var destSubDir = Path.Combine(destinationDir, Path.GetFileName(dir));
                PluginsService.CopyDirectory(dir, destSubDir);
            }
        }
    }
}
