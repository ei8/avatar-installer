using ei8.Avatar.Installer.Application.Settings;
using ei8.Avatar.Installer.Domain.Model.Configuration;
using ei8.Avatar.Installer.Domain.Model.Template;
using LibGit2Sharp;

namespace ei8.Avatar.Installer.IO.Process.Services.Template
{
    public class GithubTemplateService : ITemplateService
    {
        private readonly ISettingsService settingsService;

        public GithubTemplateService(ISettingsService settingsService) 
        {
            this.settingsService = settingsService;
        }

        public void DownloadTemplate(string destinationPath)
        {
            Repository.Clone(settingsService.TemplateDownloadUrl, destinationPath, new CloneOptions()
            {
                RepositoryOperationStarting = HandleStarting,
                RepositoryOperationCompleted = HandleComplete,
                OnCheckoutProgress = HandleCheckoutProgress,
            });
        }

        public IEnumerable<string> GetTemplateFilenames(string templatePath)
        {
            if (!Directory.Exists(templatePath))
                return null;

            return Directory.EnumerateFiles(templatePath, "*.*", new EnumerationOptions()
            {
                RecurseSubdirectories = true
            });
        }

        #region git event handlers

        private void HandleCheckoutProgress(string fileName, int checkoutSteps, int totalSteps)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var percentage = checkoutSteps * 100f / totalSteps; 
                Console.WriteLine($"Creating {fileName} {checkoutSteps}/{totalSteps} ({percentage}%)");
            }
        }

        private bool HandleStarting(RepositoryOperationContext context)
        {
            Console.WriteLine($"Cloning {context.RemoteUrl} to {context.RepositoryPath}...");

            return true;
        }

        private void HandleComplete(RepositoryOperationContext context)
        {
            Console.WriteLine($"Cloned {context.RemoteUrl} to {context.RepositoryPath} successfully.");
        }
        #endregion
    }
}