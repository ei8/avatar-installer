using System.Runtime.Serialization;

namespace ei8.Avatar.Installer.Domain.Model.Avatars.Settings;

public class EventSourcingSettings
{
    public string DatabasePath { get; set; }
    public bool DisplayErrorTraces { get; set; }
    public string PrivateKeyPath { get; set; }

    [IgnoreEnvironmentVariable]
    public string InProcessPrivateKeyPath { get; set; }
    public bool EncryptionEnabled { get; set; }

    [IgnoreEnvironmentVariable]
    public string EncryptedEventsKey { get; set; }
}
