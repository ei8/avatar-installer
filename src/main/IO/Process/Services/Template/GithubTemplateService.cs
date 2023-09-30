using ei8.Avatar.Installer.Application.Settings;
using ei8.Avatar.Installer.Domain.Model.Template;
using LibGit2Sharp;

namespace ei8.Avatar.Installer.IO.Process.Services.Template
{
    public class GithubTemplateService : ITemplateService
    {
        private readonly ISettingsService _settingsService;

        public GithubTemplateService(ISettingsService settingsService) 
        {
            _settingsService = settingsService;
        }

        public void RetrieveTemplate(string destinationPath)
        {
            Repository.Clone(_settingsService.TemplateDownloadUrl, destinationPath);
        }
    }
}