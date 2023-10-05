using ei8.Avatar.Installer.Application.Settings;
using ei8.Avatar.Installer.Domain.Model.Configuration;
using ei8.Avatar.Installer.Domain.Model.Template;
using ei8.Avatar.Installer.IO.Process.Services.Settings;
using ei8.Avatar.Installer.IO.Process.Services.Template;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ei8.Avatar.Installer.CLI
{
    internal class Program
    {
        // commandline args
        // --config = config path (JSON file, DB, etc, can be relative or absolute)
        static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddScoped<ISettingsService, SettingsService>()
                            .AddScoped<ITemplateService, GithubTemplateService>()
                            .AddScoped<IConfigurationRepository, JsonConfigurationRepository>();

            using (IHost host = builder.Build())
            {
                // entry point here
                var templateService = host.Services.GetRequiredService<ITemplateService>();
                var destinationPath = builder.Configuration.GetValue<string>("destination");

                if (string.IsNullOrEmpty(destinationPath))
                    destinationPath = ".";

                // TODO: Test parsing of JSON config

                var configRepository = host.Services.GetRequiredService<IConfigurationRepository>();
                var configPath = builder.Configuration.GetValue<string>("config");

                if (string.IsNullOrEmpty(configPath))
                    configPath = ".";

                var configObject = await configRepository.GetByAsync(configPath);

                foreach (var item in configObject.Avatars)
                {
                    // TODO: Implement loop
                    // foreach avatar - call DownloadTemplate
                    // destinationPath = root destination path + avatar name
                    templateService.DownloadTemplate(configObject.DestinationPath);

                    //var avatar = await avatarRepo.GetByAsync(item.Name);
                    //avatarMapperService.Apply(item, avatar);
                    //await avatarRepo.SaveAsync(avatar);

                    // confirm all files are present
                    templateService.GetTemplateFilenames(destinationPath);
                }

                // TODO: create avatar server here
            }
        }
    }
}