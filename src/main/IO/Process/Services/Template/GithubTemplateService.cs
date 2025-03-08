using ei8.Avatar.Installer.Domain.Model.Template;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;
using neurUL.Common.Domain.Model;

namespace ei8.Avatar.Installer.IO.Process.Services.Template
{
    public class GithubTemplateService : ITemplateService
    {
        private readonly ILogger<GithubTemplateService> logger;

        public GithubTemplateService(ILogger<GithubTemplateService> logger)
        {
            this.logger = logger;
        }

        public void DownloadTemplate(string destinationPath, string templateUrl)
        {
            AssertionConcern.AssertArgumentNotNull(destinationPath, nameof(destinationPath));
            AssertionConcern.AssertArgumentNotNull(templateUrl, nameof(templateUrl));

            Repository.Clone(
                templateUrl,
                destinationPath,
                new CloneOptions()
                {
                    RepositoryOperationStarting = HandleStarting,
                    RepositoryOperationCompleted = HandleComplete,
                    OnCheckoutProgress = HandleCheckoutProgress,
                }
            );
        }

        public IEnumerable<string> GetTemplateFilenames(string templatePath)
        {
            if (!Directory.Exists(templatePath))
                return null;

            return Directory.EnumerateFiles(
                templatePath,
                "*.*",
                new EnumerationOptions() { RecurseSubdirectories = true }
            );
        }

        #region git event handlers

        private void HandleCheckoutProgress(string fileName, int checkoutSteps, int totalSteps)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var percentage = checkoutSteps * 100f / totalSteps;
                logger.LogInformation(
                    $"Creating {fileName} {checkoutSteps}/{totalSteps} ({percentage}%)"
                );
            }
        }

        private bool HandleStarting(RepositoryOperationContext context)
        {
            logger.LogInformation($"Cloning {context.RemoteUrl} to {context.RepositoryPath}...");

            return true;
        }

        private void HandleComplete(RepositoryOperationContext context)
        {
            logger.LogInformation(
                $"Cloned {context.RemoteUrl} to {context.RepositoryPath} successfully."
            );
        }
        #endregion
    }
}
