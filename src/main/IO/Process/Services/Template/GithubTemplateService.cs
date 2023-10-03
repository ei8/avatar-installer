using ei8.Avatar.Installer.Application.Settings;
using ei8.Avatar.Installer.Domain.Model.Template;
using LibGit2Sharp;

namespace ei8.Avatar.Installer.IO.Process.Services.Template
{
    public class GithubTemplateService : ITemplateService
    {
        private readonly ISettingsService _settingsService;
        private bool _isBusy;

        public GithubTemplateService(ISettingsService settingsService) 
        {
            _settingsService = settingsService;
            _isBusy = false;
        }

        public async Task RetrieveTemplateAsync(string destinationPath)
        {
            await Task.Run(() =>
            {
                Repository.Clone(_settingsService.TemplateDownloadUrl, destinationPath, new CloneOptions()
                {
                    RepositoryOperationStarting = HandleStarting,
                    RepositoryOperationCompleted = HandleComplete,
                    OnCheckoutProgress = HandleCheckoutProgress,
                });

                while (_isBusy)
                {
                    Task.Delay(500);
                }
            });
        }

        public void EnumerateTemplateFiles(string templatePath)
        {
            if (!Directory.Exists(templatePath))
                return;

            var files = Directory.EnumerateFiles(templatePath, "*.*", new EnumerationOptions()
            {
                RecurseSubdirectories = true
            });

            foreach (var file in files)
            {
                Console.WriteLine(Path.GetFullPath(file));
            }
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

            _isBusy = true;
            return true;
        }

        private void HandleComplete(RepositoryOperationContext context)
        {
            Console.WriteLine($"Cloned {context.RemoteUrl} to {context.RepositoryPath} successfully.");
            _isBusy = false;
        }
        #endregion
    }
}