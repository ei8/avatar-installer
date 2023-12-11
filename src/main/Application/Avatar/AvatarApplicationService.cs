using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Domain.Model.Configuration;
using ei8.Avatar.Installer.Domain.Model.Mapping;
using ei8.Avatar.Installer.Domain.Model.Template;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Application.Avatar
{
    public class AvatarApplicationService : IAvatarApplicationService
    {
        private IConfigurationRepository configurationRepository;
        private IConfiguration configuration;
        private ILogger<AvatarApplicationService> logger;
        private IAvatarRepository avatarRepository;
        private IAvatarMapperService avatarMapperService;
        private ITemplateService templateService;
        private IAvatarServerRepository avatarServerRepository;
        private IAvatarServerMapperService avatarServerMapperService;

        public AvatarApplicationService(IConfigurationRepository configurationRepository, IConfiguration configuration, 
            ILogger<AvatarApplicationService> logger, IAvatarRepository avatarRepository, IAvatarMapperService avatarMapperService, 
            ITemplateService templateService, IAvatarServerRepository avatarServerRepository, IAvatarServerMapperService avatarServerMapperService)
        {
            this.configurationRepository = configurationRepository;
            this.configuration = configuration;
            this.logger = logger;
            this.avatarRepository = avatarRepository;
            this.avatarMapperService = avatarMapperService;
            this.templateService = templateService;
            this.avatarServerRepository = avatarServerRepository;
            this.avatarServerMapperService = avatarServerMapperService;
        }

        public async Task CreateAvatarAsync()
        {
            var configPath = configuration.GetSection("config").Value;

            if (string.IsNullOrEmpty(configPath))
                configPath = ".";

            var configObject = await configurationRepository.GetByAsync(configPath);

            foreach (var item in configObject.Avatars)
            {
                logger.LogInformation("Setting up avatar {itemName}", item.Name);

                var subdirectory = Path.Combine(configObject.Destination, item.Name);

                if (Directory.Exists(subdirectory) && Directory.GetFiles(subdirectory).Any())
                    logger.LogInformation("{subdirectory} is not empty. Using existing files.", subdirectory);
                else
                    templateService.DownloadTemplate(subdirectory);

                var avatar = await avatarRepository.GetByAsync(subdirectory);

                var mappedAvatar = avatarMapperService.Apply(item, avatar);

                mappedAvatar.Id = subdirectory; // is this necessary?

                await avatarRepository.SaveAsync(mappedAvatar);
                //await avatarRepository.SaveAsync(subdirectory, mappedAvatar);
            }

            var avatarServer = await avatarServerRepository.GetByAsync(configObject.Destination);
            var mappedAvatarServer = avatarServerMapperService.Apply(configObject, avatarServer);

            await avatarServerRepository.SaveAsync(configObject.Destination, mappedAvatarServer!);
        }
    }
}
