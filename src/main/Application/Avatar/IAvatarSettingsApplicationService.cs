using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Domain.Model.Avatars.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Application.Avatar;

public interface IAvatarSettingsApplicationService
{
    Task<AvatarSettings> GetAsync();
    Task SaveAsync(AvatarSettings avatarSettings);
}
