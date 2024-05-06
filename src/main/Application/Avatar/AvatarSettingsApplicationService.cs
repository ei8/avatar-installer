using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Domain.Model.Avatars.Settings;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Application.Avatar;

public class AvatarSettingsApplicationService : IAvatarSettingsApplicationService
{
    private readonly IAvatarSettingsRepository avatarSettingsRepository;

    public AvatarSettingsApplicationService(IAvatarSettingsRepository avatarSettingsRepository)
    {
        AssertionConcern.AssertArgumentNotNull(avatarSettingsRepository, nameof(avatarSettingsRepository));

        this.avatarSettingsRepository = avatarSettingsRepository;
    }
    public async Task<AvatarSettings> GetAsync()
    {
        return await avatarSettingsRepository.GetAsync();
    }

    public async Task SaveAsync(AvatarSettings avatarSettings)
    {
        AssertionConcern.AssertArgumentNotNull(avatarSettings, nameof(avatarSettings));

        await avatarSettingsRepository.SaveAsync(avatarSettings);
    }
}
