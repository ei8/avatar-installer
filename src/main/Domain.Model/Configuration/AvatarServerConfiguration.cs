using neurUL.Common.Domain.Model;
using System.Text.Json.Serialization;

namespace ei8.Avatar.Installer.Domain.Model.Configuration
{
    /// <summary>
    /// Defines how an <see cref="Avatars.AvatarItem"/> should be configured
    /// </summary>
    public class AvatarServerConfiguration
    {
        public AvatarConfigurationItem[] Avatars { get; set; }
        public string Destination { get; set; }
        public string TemplateUrl { get; set; }

        [JsonConstructor]
        public AvatarServerConfiguration()
        {
            this.Avatars = Array.Empty<AvatarConfigurationItem>();
            this.Destination = ei8.Avatar.Installer.Common.Constants.Paths.Avatars;
            this.TemplateUrl = ei8.Avatar.Installer.Common.Constants.Urls.DefaultTemplateDownloadUrl;
        }
    }

    public class AvatarConfigurationItem
    {
        /// <summary>
        /// The name of the neurULized Avatar instance.
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// The user id which is mapped to the neurULized Avatar instance in Iden8y.
        /// </summary>
        public string OwnerUserId { get; set; }
        public EventSourcingConfiguration EventSourcing { get; set; }
        public CortexGraphPersistenceConfiguration CortexGraphPersistence { get; set; }
        public CortexGraphConfiguration CortexGraph { get; set; }
        public AvatarApiConfiguration AvatarApi { get; set; }
        public CortexLibraryConfiguration CortexLibrary { get; set; }
        public Un8yConfiguration Un8y { get; set; }
        public OrchestrationConfiguration Orchestration { get; set; }
        public CortexChatNucleusConfiguration CortexChatNucleus { get; set; }
        public SshConfiguration Ssh { get; set; }

        [JsonConstructor]
        public AvatarConfigurationItem()
        {
            this.EventSourcing = new();
            this.CortexGraphPersistence = new();
            this.CortexGraph = new();
            this.AvatarApi = new();
            this.CortexLibrary = new();
            this.Un8y = new();
            this.Orchestration = new();
            this.CortexChatNucleus = new();
            this.Ssh = new();
        }
    }

    public class EventSourcingConfiguration
    {
        public string PrivateKeyPath { get; set; }
        public string InProcessPrivateKeyPath { get; set; }
        public bool EncryptionEnabled { get; set; }
        public string EncryptedEventsKey { get; set; }

        public EventSourcingConfiguration()
        {
            this.PrivateKeyPath = "/C/keys/private.key";
            this.InProcessPrivateKeyPath = string.Empty;
            this.EncryptionEnabled = false;
            this.EncryptedEventsKey = string.Empty;
        }
    }

    public class CortexGraphPersistenceConfiguration
    {
        public string ArangoRootPassword { get; set; }

        /// <summary>
        /// Initialize with defaults
        /// </summary>
        public CortexGraphPersistenceConfiguration()
        {
            this.ArangoRootPassword = string.Empty;
        }
    }

    public class CortexGraphConfiguration
    {
        public string DbName { get; set; }
        public string DbUsername { get; set; }
        public string DbUrl { get; set; }

        /// <summary>
        /// Initialize with defaults
        /// </summary>
        [JsonConstructor]
        public CortexGraphConfiguration()
        {
            this.DbName = "graph_${AVATAR_NAME}";
            this.DbUsername = "root";
            this.DbUrl = "http://cortex.graph.persistence:8529";
        }
    }

    public class AvatarApiConfiguration
    {
        public string AnonymousUserId { get; set; }
        public string TokenIssuerAddress { get; set; }
        public string ApiName { get; set; }

        /// <summary>
        /// Initialize with defaults
        /// </summary>
        [JsonConstructor]
        public AvatarApiConfiguration()
        {
            this.AnonymousUserId = "Guest";
            this.TokenIssuerAddress = @"https://login.fibona.cc";
            this.ApiName = "avatarapi-${AVATAR_NAME}";
        }
    }

    public class CortexLibraryConfiguration
    {
        public string NeuronsUrl { get; set; }
        public string TerminalsUrl { get; set; }

        /// <summary>
        /// Initialize with defaults
        /// </summary>
        [JsonConstructor]
        public CortexLibraryConfiguration()
        {
            this.NeuronsUrl = @"https://fibona.cc/${AVATAR_NAME}/cortex/neurons";
            this.TerminalsUrl = @"https://fibona.cc/${AVATAR_NAME}/cortex/terminals";
        }
    }

    public class Un8yConfiguration
    {
        public string OidcAuthorityUrl { get; set; }
        public string ClientId { get; set; }
        public string RequestedScopes { get; set; }
        public string BasePath { get; set; }
        public string CertificatePassword { get; set; }
        public string CertificatePath { get; set; }
        public PluginConfiguration[] Plugins { get; set; }

        /// <summary>
        /// Initialize with defaults
        /// </summary>
        [JsonConstructor]
        public Un8yConfiguration()
        {
            this.OidcAuthorityUrl = @"https://login.fibona.cc";
            this.ClientId = "un8y-${AVATAR_NAME}";
            this.RequestedScopes = "openid,profile,email,avatarapi-${AVATAR_NAME},offline_access";
            this.BasePath = "/${AVATAR_NAME}/un8y";
            this.CertificatePassword = string.Empty;
            this.CertificatePath = "/https/aspnetapp.pfx";
            this.Plugins = Array.Empty<PluginConfiguration>();
        }
    }

    public class PluginConfiguration
    {
        private string name;
        private string url;

        public string Name
        {
            get => name;
            set
            {
                AssertionConcern.AssertArgumentNotNull(value, nameof(Name));
                AssertionConcern.AssertArgumentNotEmpty(value, $"'{nameof(Name)}' cannot be empty.", nameof(Name));
                name = value;
            }
        }

        public string Url
        {
            get => url;
            set
            {
                AssertionConcern.AssertArgumentNotNull(value, nameof(Url));
                AssertionConcern.AssertArgumentNotEmpty(value, $"'{nameof(Url)}' cannot be empty.", nameof(Url));
                url = value;
            }
        }

        /// <summary>
        /// Subfolder to extract from the downloaded archive. Leave empty to extract everything.
        /// </summary>
        public string Path { get; set; }

        [JsonConstructor]
        public PluginConfiguration()
        {
            this.name = string.Empty;
            this.url = string.Empty;
            this.Path = string.Empty;
        }
    }

    public class OrchestrationConfiguration
    {
        public string AvatarName { get; set; }
        public int TunnelLocalPort { get; set; }
        public string KeysPath { get; set; }
        public int GraphPersistencePort { get; set; }

        /// <summary>
        /// Initialize with defaults
        /// </summary>

        [JsonConstructor]
        public OrchestrationConfiguration()
        {
            this.AvatarName = "sample";
            this.TunnelLocalPort = 9393;
            this.KeysPath = string.Empty;
            this.GraphPersistencePort = 8529;
        }
    }

    public class CortexChatNucleusConfiguration
    {
        public int PageSize { get; set; }
        public bool InitializeMissingMirrors { get; set; }

        [JsonConstructor]
        public CortexChatNucleusConfiguration()
        {
            this.PageSize =  10;
            this.InitializeMissingMirrors = true;
        }
    }

    public class SshConfiguration
    {
        public int ServerAliveInterval { get; set; }
        public int ServerAliveCountMax { get; set; }
        public int Port { get; set; }
        public string HostName { get; set; }

        /// <summary>
        /// Initialize with defaults
        /// </summary>
        [JsonConstructor]
        public SshConfiguration()
        {
            this.ServerAliveInterval = 60;            // 1 minute
            this.ServerAliveCountMax = 60 * 24 * 365; // 60 secs * 24 hours * 365 days = 1 year
            this.Port = 2222;
            this.HostName = "ei8.host";
        }
    }
}
