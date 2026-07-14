using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Domain.Model.Configuration;
using ei8.Avatar.Installer.Domain.Model.Mapping;
using ei8.Avatar.Installer.Domain.Model.Plugins;
using ei8.Avatar.Installer.Domain.Model.Template;
using Microsoft.Extensions.Logging;
using neurUL.Common.Domain.Model;
using System.Diagnostics;

namespace ei8.Avatar.Installer.Application.Avatar
{
    public class AvatarApplicationService : IAvatarApplicationService
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IProgressService progressService;
        private readonly ILogger<AvatarApplicationService> logger;
        private readonly IAvatarItemReadRepository avatarItemReadRepository;
        private readonly IAvatarItemWriteRepository avatarItemWriteRepository;
        private readonly IAvatarMapperService avatarMapperService;
        private readonly ITemplateService templateService;
        private readonly IPluginsService pluginsService;

        public AvatarApplicationService(
            IConfigurationRepository configurationRepository,
            IProgressService progressService,
            ILogger<AvatarApplicationService> logger,
            IAvatarItemReadRepository avatarItemReadRepository,
            IAvatarItemWriteRepository avatarItemWriteRepository,
            IAvatarMapperService avatarMapperService,
            ITemplateService templateService,
            IPluginsService pluginsService
        )
        {
            this.configurationRepository = configurationRepository;
            this.progressService = progressService;
            this.logger = logger;
            this.avatarItemReadRepository = avatarItemReadRepository;
            this.avatarItemWriteRepository = avatarItemWriteRepository;
            this.avatarMapperService = avatarMapperService;
            this.templateService = templateService;
            this.pluginsService = pluginsService;
        }

        public async Task<AvatarServerConfiguration> ReadAvatarConfiguration(string configPath)
        {
            AssertionConcern.AssertArgumentNotNull(configPath, nameof(configPath));

            this.progressService.Reset();
            this.progressService.Update(0.1, "Creating Avatar...");

            var configObject = await configurationRepository.GetByIdAsync(configPath);

            return configObject;
        }

        public async Task CreateAvatarAsync(AvatarServerConfiguration configObject)
        {
            AssertionConcern.AssertArgumentNotNull(configObject, nameof(configObject));

            this.progressService.Update(0.3, "Configuring Avatar...");
            foreach (var item in configObject.Avatars)
            {
                logger.LogInformation("Setting up avatar {itemName}", item.Orchestration.AvatarName);

                var subdirectory = Path.Combine(configObject.Destination, item.Orchestration.AvatarName);
                var templateUrl = configObject.TemplateUrl;

                if (Directory.Exists(subdirectory) && Directory.GetFiles(subdirectory).Any())
                    logger.LogInformation(
                        "{subdirectory} is not empty. Using existing files.",
                        subdirectory
                    );
                else
                    templateService.DownloadTemplate(subdirectory, templateUrl);

                foreach (var plugin in item.Un8y.Plugins)
                {
                    var pluginDirectory = Path.Combine(
                        subdirectory,
                        Common.Constants.Directories.Un8y,
                        Common.Constants.Directories.Un8yPlugins,
                        plugin.Name
                    );
                    if (Directory.Exists(pluginDirectory) && Directory.GetFiles(pluginDirectory).Any())
                        logger.LogInformation(
                            "{pluginDirectory} is not empty. Using existing plugin '{pluginName}'.",
                            pluginDirectory,
                            plugin.Name
                        );
                    else
                        await pluginsService.DownloadAndExtractAsync(pluginDirectory, plugin.Url);
                }

                var avatar = await this.avatarItemReadRepository.GetByAsync(subdirectory);

                var mappedAvatar = avatarMapperService.Apply(item, avatar);

                await this.TryGenerateHttpsCertificateAsync(item.Un8y, subdirectory);

                this.progressService.Update(0.8, "Saving Avatar...");

                await this.avatarItemWriteRepository.SaveAsync(mappedAvatar);

                
            }

            this.progressService.Update(1.0, "Finished Creating Avatars!");
        }

        private async Task TryGenerateHttpsCertificateAsync(Un8yConfiguration un8yConfiguration, string avatarDirectory)
        {
            if (un8yConfiguration == null)
            {
                throw new ArgumentNullException(nameof(un8yConfiguration));
            }

            if (!string.IsNullOrWhiteSpace(un8yConfiguration.CertificatePath))
            {
                if (string.IsNullOrWhiteSpace(un8yConfiguration.CertificatePassword))
                {
                    logger.LogInformation(
                        "Skipping HTTPS certificate generation for '{certificatePath}' because certificate password is empty.",
                        un8yConfiguration.CertificatePath
                    );
                }
                else
                {
                    var normalizedCertificatePath = PathHelper.NormalizeRelativePath(un8yConfiguration.CertificatePath);
                    var certificateFilePath = Path.Combine(avatarDirectory, normalizedCertificatePath);
                    var certificateDirectory = Path.GetDirectoryName(certificateFilePath);

                    if (!string.IsNullOrWhiteSpace(certificateDirectory))
                    {
                        Directory.CreateDirectory(certificateDirectory);
                    }

                    logger.LogInformation("Generating HTTPS development certificate at '{certificateFilePath}'.", certificateFilePath);

                    var processStartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = $"dev-certs https -ep \"{certificateFilePath}\" -p \"{un8yConfiguration.CertificatePassword}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using var process = new Process { StartInfo = processStartInfo };
                    process.Start();
                    await process.WaitForExitAsync();

                    var standardOutput = await process.StandardOutput.ReadToEndAsync();
                    var standardError = await process.StandardError.ReadToEndAsync();

                    if (process.ExitCode != 0)
                    {
                        throw new InvalidOperationException(
                            $"Failed to generate HTTPS development certificate. Command: dotnet dev-certs https -ep \"{certificateFilePath}\" -p \"{un8yConfiguration.CertificatePassword}\". Error: {standardError}"
                        );
                    }

                    logger.LogInformation("HTTPS development certificate generated successfully. {stdout}", standardOutput);
                }
            }
            else
            {
                logger.LogInformation("Skipping HTTPS certificate generation because no certificate path was provided.");
            }
        }

    }
}
