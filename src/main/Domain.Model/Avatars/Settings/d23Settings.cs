using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.Avatars.Settings;

public class d23Settings
{
    public string OidcAuthority { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public int UpdateCheckInterval { get; set; }
    public string DatabasePath { get; set; }
    public string BasePath { get; set; }
    public string PluginsPath { get; set; }
    public bool ValidateServerCertificate { get; set; }
    public string AppTitle { get; set; }
    public string AppIcon { get; set; }
}
