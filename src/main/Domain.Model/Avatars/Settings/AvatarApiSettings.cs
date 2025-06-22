namespace ei8.Avatar.Installer.Domain.Model.Avatars.Settings;

public class AvatarApiSettings
{
    public string ResourceDatabasePath { get; set; }
    public bool RequireAuthentication { get; set; }
    public string AnonymousUserId { get; set; }
    public Guid ProxyUserId { get; set; }
    public string TokenIssuerAddress { get; set; }
    public string ApiName { get; set; }
    public string ApiSecret { get; set; }
    public bool ValidateServerCertificate { get; set; }
}
