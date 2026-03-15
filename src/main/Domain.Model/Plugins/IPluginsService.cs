namespace ei8.Avatar.Installer.Domain.Model.Plugins
{
    public interface IPluginsService
    {
        /// <summary>
        /// Download a ZIP archive from <paramref name="url"/> and extract its contents into <paramref name="destinationPath"/>.
        /// When <paramref name="subPath"/> is specified, only that subfolder from the archive is extracted.
        /// </summary>
        Task DownloadAndExtractAsync(string destinationPath, string url, string subPath);
    }
}
