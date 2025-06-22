using ei8.Avatar.Installer.Application.Settings;
using ei8.Cortex.Coding;

namespace ei8.Avatar.Installer.IO.Process.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        public IEnumerable<MirrorConfig> Mirrors { get; set; }
    }
}
