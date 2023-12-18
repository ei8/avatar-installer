using neurUL.Common.Domain.Model;

namespace ei8.Avatar.Installer.Domain.Model.Avatars
{
    /// <summary>
    /// A concrete representation of an avatar, derived from a <see cref="Configuration.AvatarConfiguration"/>
    /// </summary>
    public class AvatarItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SpecifiedName { get; set; }
        public EventSourcingSettings EventSourcing { get; set; } = new();
        public CortexGraphSettings CortexGraph { get; set; } = new();
        public AvatarApiSettings AvatarApi { get; set; } = new();
        public IdentityAccessSettings IdentityAccess { get; set; } = new();
        public CortexLibrarySettings CortexLibrary { get; set; } = new();
        public CortexDiaryNucleusSettings CortexDiaryNucleus { get; set; } = new();
        public d23Settings d23 { get; set; } = new();
        public AvatarNetworkSettings Network { get; set; } = new();

        public AvatarItem(string id, string name, string specifiedName = null)
        {
            AssertionConcern.AssertArgumentNotEmpty(id, "Specified 'id' cannot be empty.", nameof(id));
            AssertionConcern.AssertArgumentNotNull(id, nameof(id));

            AssertionConcern.AssertArgumentNotEmpty(name, "Specified 'name' cannot be empty.", nameof(name));
            AssertionConcern.AssertArgumentNotNull(name, nameof(name));

            this.Id = id;
            this.Name = name;
            this.SpecifiedName = specifiedName;
        }
    }

    public class EventSourcingSettings
    {
        public string DatabasePath { get; set; }
        public bool DisplayErrorTraces { get; set; }
    }

    public class CortexGraphSettings
    {
        public int PollInterval { get; set; }
        public string DbName { get; set; }
        public string DbUsername { get; set; }
        public string DbPassword { get; set; }
        public string DbUrl { get; set; }
        public int DefaultRelativeValues { get; set; }
        public int DefaultNeuronActiveValues { get; set; }
        public int DefaultTerminalActiveValues { get; set; }
        public int DefaultPageSize { get; set; }
        public int DefaultPage { get; set; }
        public string ArangoRootPassword { get; set; }
    }

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

    public class IdentityAccessSettings
    {
        public string UserDatabasePath { get; set; }
    }

    public class CortexLibrarySettings
    {
        public string NeuronsUrl { get; set; }
        public string TerminalsUrl { get; set; }
    }

    public class CortexDiaryNucleusSettings
    {
        public string SubscriptionsDatabasePath { get; set; }
        public int SubscriptionsPollingIntervalSecs { get; set; }
        public string SubscriptionsPushOwner { get; set; }
        public string SubscriptionsPushPublicKey { get; set; }
        public string SubscriptionsPushPrivateKey { get; set; }
        public string SubscriptionsSmtpServerAddress { get; set; }
        public int SubscriptionsSmtpPort { get; set; }
        public bool SubscriptionsSmtpUseSsl { get; set; }
        public string SubscriptionsSmtpSenderName { get; set; }
        public string SubscriptionsSmtpSenderAddress { get; set; }
        public string SubscriptionsSmtpSenderUsername { get; set; }
        public string SubscriptionsSmtpSenderPassword { get; set; }
        public string SubscriptionsCortexGraphOutBaseUrl { get; set; }
    }

    public class PlatformSettings
    {
        public string AspnetcoreEnvironment { get; set; }
    }
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

    public class AvatarNetworkSettings
    {
        public string AvatarIp { get; set; }
        public string D23Ip { get; set; }
        public int AvatarInPort { get; set; }
        public int D23BlazorPort { get; set; }
    }
}
