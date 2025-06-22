namespace ei8.Avatar.Installer.Domain.Model.Avatars.Settings;

public class Un8ySettings
{
    public string OidcAuthority { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string RequestedScopes { get; set; }
    public int UpdateCheckInterval { get; set; }
    public string DatabasePath { get; set; }
    public string BasePath { get; set; }
    public string PluginsPath { get; set; }
    public string MirrorConfigFiles { get; set; }
    public bool ValidateServerCertificate { get; set; }
    public string AppTitle { get; set; }
    public string AppIcon { get; set; }
}
