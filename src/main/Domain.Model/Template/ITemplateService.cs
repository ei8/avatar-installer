namespace ei8.Avatar.Installer.Domain.Model.Template
{
    public interface ITemplateService
    {
        /// <summary>
        /// Download template files from a remote source into a local directory
        /// </summary>
        /// <param name="destinationPath"></param>
        /// <returns></returns>
        void DownloadTemplate(string destinationPath);

        /// <summary>
        /// Enumerate all template files in the local directory
        /// </summary>
        /// <param name="templatePath"></param>
        IEnumerable<string> GetTemplateFilenames(string templatePath);
    }
}