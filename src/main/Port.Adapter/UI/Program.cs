using ei8.Avatar.Installer.Application.Settings;
using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Domain.Model.Configuration;
using ei8.Avatar.Installer.Domain.Model.Mapping;
using ei8.Avatar.Installer.Domain.Model.Template;
using ei8.Avatar.Installer.IO.Process.Services.Avatars;
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
            try
            {
                var builder = Host.CreateApplicationBuilder(args);

                builder.Services.AddScoped<ISettingsService, SettingsService>()
                                .AddScoped<ITemplateService, GithubTemplateService>()
                                .AddScoped<IConfigurationRepository, JsonConfigurationRepository>()
                                .AddScoped<IAvatarRepository, AvatarRepository>()
                                .AddScoped<IAvatarMapperService, AvatarMapperService>();

                using (IHost host = builder.Build())
                {
                    // entry point here
                    var templateService = host.Services.GetRequiredService<ITemplateService>();

                    var configRepository = host.Services.GetRequiredService<IConfigurationRepository>();
                    var configPath = builder.Configuration.GetValue<string>("config");

                    if (string.IsNullOrEmpty(configPath))
                        configPath = ".";

                    var configObject = await configRepository.GetByAsync(configPath);

                    var avatarRepository = host.Services.GetRequiredService<IAvatarRepository>();

                    foreach (var item in configObject.Avatars)
                    {
                        // TODO: Move loop logic into an application service

                        var subdirectory = Path.Combine(configObject.Destination, item.Name);

                        if (Directory.Exists(subdirectory) && Directory.GetFiles(subdirectory).Any())
                            Console.WriteLine($"{subdirectory} is not empty. Using existing files.");
                        else
                            templateService.DownloadTemplate(subdirectory);

                        var avatar = await avatarRepository.GetByAsync(subdirectory);
                        //avatarMapperService.Apply(item, avatar);
                        //await avatarRepo.SaveAsync(avatar);
                    }

                    // TODO: create avatar server here
                    // var avatarServer = await avatarServerRepository.Get();
                    // await avatarServerMapperService.Apply(configObject, avatarServer);
                    // await avatarServerRepo.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Complete. Press any key to exit.");
                Console.ReadKey();
            }
        }
    }
}