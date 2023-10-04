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
        // --destination = destination path (can be relative or absolute)
        // --config = config path (can be relative or absolute)
        static void Main(string[] args)
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

                templateService.RetrieveTemplate(destinationPath);

                // TODO US#3
                // templateService.GenerateLocalFiles(destinationPath);

                // confirm all files are present
                templateService.EnumerateTemplateFiles(destinationPath);

                var configRepository = host.Services.GetRequiredService<IConfigurationRepository>();
                var configPath = builder.Configuration.GetValue<string>("config");

                if (string.IsNullOrEmpty(configPath)) 
                    configPath = ".";

                // configRepository.ReadAllAsync(configPath);
            }
        }
    }
}