using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Domain.Model.Avatars.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.AvatarSettings;

// TODO: rename to AvatarSettingsRepository
/// <summary>
/// Load and save whole file of variables.env
/// </summary>
public interface IEventSourcingSettingsRepository
{
    Task<EventSourcingSettings> GetAsync();
    Task SaveAsync(EventSourcingSettings eventSourcingSettings);
}
