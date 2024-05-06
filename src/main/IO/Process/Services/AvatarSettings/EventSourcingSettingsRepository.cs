using ei8.Avatar.Installer.Application.Avatar;
using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model;
using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Domain.Model.Avatars.Settings;
using ei8.Avatar.Installer.Domain.Model.AvatarSettings;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.IO.Process.Services.AvatarSettings;

public class EventSourcingSettingsRepository : IEventSourcingSettingsRepository
{
    private readonly IAvatarContextService avatarContextService;

    public EventSourcingSettingsRepository(IAvatarContextService avatarContextService)
    {
        AssertionConcern.AssertArgumentNotNull(avatarContextService, nameof(avatarContextService));

        this.avatarContextService = avatarContextService;
    }

    public async Task<EventSourcingSettings> GetAsync()
    {
        var path = Path.Combine(this.avatarContextService.Avatar.Id, Constants.Filenames.VariablesEnv);
        var eventSourcingSettings = new EventSourcingSettings();

        var envFile = await File.ReadAllLinesAsync(path);

        foreach (var line in envFile)
        {
            if (string.IsNullOrWhiteSpace(line) || line.Trim().StartsWith("#"))
                continue;

            var index = line.IndexOf('=');

            // If no "=" found, go to the next line
            if (index == -1) continue;

            var key = line[..index].Trim();
            var value = line[(index + 1)..].Trim();

            switch (key.ToUpper())
            {
                case Constants.EventSourcingSettings.DatabasePath:
                    eventSourcingSettings.DatabasePath = value;
                    break;
                case Constants.EventSourcingSettings.DisplayErrorTraces:
                    bool displayErrorTraces;
                    if (bool.TryParse(value, out displayErrorTraces))
                        eventSourcingSettings.DisplayErrorTraces = displayErrorTraces;
                    break;
                default:
                    break;
            }
        }

        return eventSourcingSettings;
    }

    // public async Task SaveAsync(AvatarSettings avatarSettings)
    public async Task SaveAsync(EventSourcingSettings eventSourcingSettings)
    {
        AssertionConcern.AssertArgumentNotNull(eventSourcingSettings, nameof(eventSourcingSettings));

        var path = Path.Combine(this.avatarContextService.Avatar.Id, Constants.Filenames.VariablesEnv);
        var envVariables = new Dictionary<string, string>();

        var envFile = await File.ReadAllLinesAsync(path);

        foreach (var line in envFile)
        {
            if (string.IsNullOrWhiteSpace(line) || line.Trim().StartsWith("#"))
                continue;

            var index = line.IndexOf('=');

            // If no "=" found, go to the next line
            if (index == -1) continue;

            var key = line[..index].Trim();
            var value = line[(index + 1)..].Trim();

            envVariables[key] = value;
        }

        // Update key-value pairs
        envVariables[Constants.EventSourcingSettings.DatabasePath] = eventSourcingSettings.DatabasePath;
        envVariables[Constants.EventSourcingSettings.DisplayErrorTraces] = eventSourcingSettings.DisplayErrorTraces.ToString();

        // Write changes to the file
        using var writer = new StreamWriter(path);

        foreach (var keyValuePair in envVariables)
        {
            writer.WriteLine($"{keyValuePair.Key}={keyValuePair.Value}");
        }
    }
}
