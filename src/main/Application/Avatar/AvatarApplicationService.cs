using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Domain.Model.Configuration;
using ei8.Avatar.Installer.Domain.Model.Mapping;
using ei8.Avatar.Installer.Domain.Model.Template;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Application.Avatar
{
    public class AvatarApplicationService : IAvatarApplicationService
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IProgressService progressService;
        private readonly ILogger<AvatarApplicationService> logger;
        private readonly IAvatarRepository avatarRepository;
        private readonly IAvatarMapperService avatarMapperService;
        private readonly ITemplateService templateService;
        private readonly IAvatarServerRepository avatarServerRepository;
        private readonly IAvatarServerMapperService avatarServerMapperService;

        public AvatarApplicationService(IConfigurationRepository configurationRepository, IProgressService progressService,
            ILogger<AvatarApplicationService> logger, IAvatarRepository avatarRepository, IAvatarMapperService avatarMapperService,
            ITemplateService templateService, IAvatarServerRepository avatarServerRepository, IAvatarServerMapperService avatarServerMapperService)
        {
            this.configurationRepository = configurationRepository;
            this.progressService = progressService;
            this.logger = logger;
            this.avatarRepository = avatarRepository;
            this.avatarMapperService = avatarMapperService;
            this.templateService = templateService;
            this.avatarServerRepository = avatarServerRepository;
            this.avatarServerMapperService = avatarServerMapperService;
        }

        public async Task CreateAvatarAsync(string id)
        {
            AssertionConcern.AssertArgumentNotNull(id, nameof(id));

            this.progressService.Reset();
            this.progressService.Update(0.1, "Creating Avatar...");

            var configObject = await configurationRepository.GetByAsync(id);

            this.progressService.Update(0.3, "Configuring Avatar...");
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

                await avatarRepository.SaveAsync(mappedAvatar);
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
