namespace ei8.Avatar.Installer.Domain.Model.Template
{
    public interface ITemplateService
    {
        /// <summary>
        /// Download template files from a remote source into a local directory
        /// </summary>
        /// <param name="destinationPath"></param>
        /// <returns></returns>
        void RetrieveTemplate(string destinationPath);

        /// <summary>
        /// Enumerate all template files in the local directory
        /// </summary>
        /// <param name="templatePath"></param>
        void EnumerateTemplateFiles(string templatePath);

        /// <summary>
        /// Generate files locally needed for a complete template
        /// </summary>
        /// <param name="destinationPath"></param>
        /// <returns></returns>
        void GenerateLocalFiles(string destinationPath);
    }
}