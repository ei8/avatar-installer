using ei8.Avatar.Installer.IO.Process.Services.Settings;
using ei8.Avatar.Installer.IO.Process.Services.Template;

namespace ei8.Avatar.Installer.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var settingsService = new SettingsService();
            var templateService = new GithubTemplateService(settingsService);

            templateService.RetrieveTemplate(@"./template");
        }
    }
}