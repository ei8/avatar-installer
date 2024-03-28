using ei8.Avatar.Installer.Application.Settings;
using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.Template;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;

namespace ei8.Avatar.Installer.IO.Process.Services.Template
{
    public class GithubTemplateService : ITemplateService
    {
        private readonly ISettingsService settingsService;
        private readonly ILogger<GithubTemplateService> logger;

        public GithubTemplateService(ISettingsService settingsService,
            ILogger<GithubTemplateService> logger) 
        {
            this.settingsService = settingsService;
            this.logger = logger;
        }

        public void DownloadTemplate(string destinationPath)
        {
            // .NET Maui doesn't have environment variables so the url is hardcoded
            var templateDownloadUrl = settingsService.TemplateDownloadUrl ?? Constants.Urls.DefaultTemplateDownloadUrl;

            Repository.Clone(templateDownloadUrl, destinationPath, new CloneOptions()
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
                logger.LogInformation($"Creating {fileName} {checkoutSteps}/{totalSteps} ({percentage}%)");
            }
        }

        private bool HandleStarting(RepositoryOperationContext context)
        {
            logger.LogInformation($"Cloning {context.RemoteUrl} to {context.RepositoryPath}...");

            return true;
        }

        private void HandleComplete(RepositoryOperationContext context)
        {
            logger.LogInformation($"Cloned {context.RemoteUrl} to {context.RepositoryPath} successfully.");
        }
        #endregion
    }
}