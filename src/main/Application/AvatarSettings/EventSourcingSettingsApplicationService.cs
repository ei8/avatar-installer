using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Domain.Model.AvatarSettings;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Application.AvatarSettings;

public class EventSourcingSettingsApplicationService : IEventSourcingSettingsApplicationService
{
    private readonly IEventSourcingSettingsRepository eventSourcingSettingsRepository;

    public EventSourcingSettingsApplicationService(IEventSourcingSettingsRepository eventSourcingSettingsRepository)
    {
        this.eventSourcingSettingsRepository = eventSourcingSettingsRepository;
    }

    public async Task<EventSourcingSettings> GetAsync()
    {
        return await this.eventSourcingSettingsRepository.GetAsync();
    }

    public async Task SaveAsync(EventSourcingSettings eventSourcingSettings)
    {
        AssertionConcern.AssertArgumentNotNull(eventSourcingSettings, nameof(eventSourcingSettings));

        await this.eventSourcingSettingsRepository.SaveAsync(eventSourcingSettings);
    }
}
