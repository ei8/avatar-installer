using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.Avatars.Settings;

public class AvatarApiSettings
{
    public string ResourceDatabasePath { get; set; }
    public bool RequireAuthentication { get; set; }
    public Guid AnonymousUserId { get; set; }
    public Guid ProxyUserId { get; set; }
    public string TokenIssuerUrl { get; set; }
    public string ApiName { get; set; }
    public string ApiSecret { get; set; }
    public bool ValidateServerCertificate { get; set; }
}
