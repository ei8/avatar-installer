using ei8.Avatar.Installer.Domain.Model.Avatars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Application.AvatarSettings;

public interface IEventSourcingSettingsApplicationService
{
    Task<EventSourcingSettings> GetAsync();
    Task SaveAsync(EventSourcingSettings eventSourcingSettings);
}
