using ei8.Cortex.Coding;

namespace ei8.Avatar.Installer.Application.Settings
{
    public interface ISettingsService
    {
        IEnumerable<MirrorConfig> Mirrors { get; set; }
    }
}
