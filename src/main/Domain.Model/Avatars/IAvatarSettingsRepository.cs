using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.Avatars;

public interface IAvatarSettingsRepository
{
    Task<AvatarSettings> GetAsync();
    Task SaveAsync(AvatarSettings avatarSettings);
}
