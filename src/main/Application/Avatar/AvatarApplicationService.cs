﻿using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Domain.Model.Configuration;
using ei8.Avatar.Installer.Domain.Model.Mapping;
using ei8.Avatar.Installer.Domain.Model.Template;
using Microsoft.Extensions.Logging;
using neurUL.Common.Domain.Model;

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
        private readonly IAvatarServerRepository avatarServerRepository;
        private readonly IAvatarServerMapperService avatarServerMapperService;

        public AvatarApplicationService(
            IConfigurationRepository configurationRepository,
            IProgressService progressService,
            ILogger<AvatarApplicationService> logger,
            IAvatarItemReadRepository avatarItemReadRepository,
            IAvatarItemWriteRepository avatarItemWriteRepository,
            IAvatarMapperService avatarMapperService,
            ITemplateService templateService,
            IAvatarServerRepository avatarServerRepository,
            IAvatarServerMapperService avatarServerMapperService
        )
        {
            this.configurationRepository = configurationRepository;
            this.progressService = progressService;
            this.logger = logger;
            this.avatarItemReadRepository = avatarItemReadRepository;
            this.avatarItemWriteRepository = avatarItemWriteRepository;
            this.avatarMapperService = avatarMapperService;
            this.templateService = templateService;
            this.avatarServerRepository = avatarServerRepository;
            this.avatarServerMapperService = avatarServerMapperService;
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
                logger.LogInformation("Setting up avatar {itemName}", item.Name);

                var subdirectory = Path.Combine(configObject.Destination, item.Name);
                var templateUrl = configObject.TemplateUrl;

                if (Directory.Exists(subdirectory) && Directory.GetFiles(subdirectory).Any())
                    logger.LogInformation(
                        "{subdirectory} is not empty. Using existing files.",
                        subdirectory
                    );
                else
                    templateService.DownloadTemplate(subdirectory, templateUrl);

                var avatar = await this.avatarItemReadRepository.GetByAsync(subdirectory);

                var mappedAvatar = avatarMapperService.Apply(item, avatar);

                await this.avatarItemWriteRepository.SaveAsync(mappedAvatar);
            }

            this.progressService.Update(0.5, "Mapping Avatar...");
            var avatarServer = await avatarServerRepository.GetByAsync(configObject.Destination);
            var mappedAvatarServer = avatarServerMapperService.Apply(configObject, avatarServer);

            this.progressService.Update(0.8, "Saving Avatar...");
            await avatarServerRepository.SaveAsync(mappedAvatarServer);

            this.progressService.Update(1.0, "Finished Creating Avatar!");
        }
        
    }
}
