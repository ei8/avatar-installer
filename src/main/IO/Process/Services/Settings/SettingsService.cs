﻿using ei8.Avatar.Installer.Application.Settings;

namespace ei8.Avatar.Installer.IO.Process.Services.Settings
{
    // Unused service.
    public class SettingsService : ISettingsService
    {
        public string TemplateDownloadUrl => Environment.GetEnvironmentVariable("TEMPLATE_DOWNLOAD_URL");
    }
}
