using ei8.Avatar.Installer.Domain.Model;
using ei8.Avatar.Installer.IO.Process.Services.Settings;
using ei8.Avatar.Installer.IO.Process.Services.Template;

namespace ei8.Avatar.Installer.CLI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var commandLineOptions = new CommandLineOptions(args);

            var settingsService = new SettingsService();
            var templateService = new GithubTemplateService(settingsService);

            await templateService.RetrieveTemplateAsync(commandLineOptions.DestinationPath);

            templateService.EnumerateTemplateFiles(commandLineOptions.DestinationPath);
        }
    }
}