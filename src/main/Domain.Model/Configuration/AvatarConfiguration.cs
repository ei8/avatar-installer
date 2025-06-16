using ei8.Avatar.Installer.Domain.Model.Avatars.Settings;
using System.Text.Json.Serialization;

namespace ei8.Avatar.Installer.Domain.Model.Configuration
{
    /// <summary>
    /// Defines how an <see cref="Avatars.AvatarItem"/> should be configured
    /// </summary>
    public class AvatarConfiguration
    {
        public AvatarConfigurationItem[] Avatars { get; set; }
        public string Destination { get; set; }
        public string TemplateUrl { get; set; }
        public AvatarServerConfiguration Network { get; set; }

        public AvatarConfiguration(string avatarName)
        {
            this.Network = new(avatarName);
        }

        [JsonConstructor]
        public AvatarConfiguration() : this("sample")
        {
        }
    }

    public class AvatarServerConfiguration
    {
        public string LocalIp { get; set; }
        public SshConfiguration Ssh { get; set; }

        /// <summary>
        /// Initialize with defaults
        /// </summary>
        public AvatarServerConfiguration(string avatarName)
        {
            LocalIp = "192.168.1.110";
            Ssh = new(avatarName);
        }

        [JsonConstructor]
        public AvatarServerConfiguration() : this("sample") { }
    }

    public class AvatarConfigurationItem
    {
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public string OwnerUserId { get; set; }
        public CortexGraphPersistenceConfiguration CortexGraphPersistence { get; set; }
        public CortexGraphConfiguration CortexGraph { get; set; }
        public AvatarApiConfiguration AvatarApi { get; set; }
        public CortexLibraryConfiguration CortexLibrary { get; set; }
        public Un8yConfiguration Un8y { get; set; }
        public NetworkConfiguration Network { get; set; }

        [JsonConstructor]
        public AvatarConfigurationItem(string name)
        {
            this.Name = name;
            this.CortexGraphPersistence = new();
            this.CortexGraph = new(this.Name);
            this.AvatarApi = new(this.Name);
            this.CortexLibrary = new(this.Name);
            this.Un8y = new(this.Name);
            this.Network = new();
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
        public CortexGraphConfiguration(string avatarName)
        {
            DbName = $"graph_{avatarName}";
            DbUsername = "root";
            DbUrl = "http://cortex.graph.persistence:8529";
        }

        [JsonConstructor]
        public CortexGraphConfiguration() : this("sample")
        {
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
        public AvatarApiConfiguration(string avatarName)
        {
            AnonymousUserId = "Guest";
            TokenIssuerAddress = @"https://login.fibona.cc";
            ApiName = $"avatarapi-{avatarName}";
        }

        [JsonConstructor]
        public AvatarApiConfiguration() : this("sample") { }
    }

    public class CortexLibraryConfiguration
    {
        public string NeuronsUrl { get; set; }
        public string TerminalsUrl { get; set; }

        /// <summary>
        /// Initialize with defaults
        /// </summary>
        public CortexLibraryConfiguration(string avatarName)
        {
            NeuronsUrl = $@"https://fibona.cc/{avatarName}/cortex/neurons";
            TerminalsUrl = $@"https://fibona.cc/{avatarName}/cortex/terminals";
        }

        [JsonConstructor]
        public CortexLibraryConfiguration() : this("sample") { }
    }

    public class Un8yConfiguration
    {
        public string OidcAuthorityUrl { get; set; }
        public string ClientId { get; set; }
        public string RequestedScopes { get; set; }
        public string BasePath { get; set; }

        /// <summary>
        /// Initialize with defaults
        /// </summary>
        public Un8yConfiguration(string avatarName)
        {
            OidcAuthorityUrl = @"https://login.fibona.cc";
            ClientId = $"un8y-{avatarName}";
            RequestedScopes = $"openid,profile,email,avatarapi-{avatarName},offline_access";
            BasePath = $"/{avatarName}/un8y";
        }

        [JsonConstructor]
        public Un8yConfiguration() : this("sample") { }
    }

    public class NetworkConfiguration
    {
        public string LocalIp { get; set; }
        public int AvatarInPort { get; set; }
        public int Un8yBlazorPort { get; set; }
        public string NeurULServer { get; set; }

        /// <summary>
        /// Initialize with defaults
        /// </summary>

        [JsonConstructor]
        public NetworkConfiguration()
        {
            LocalIp = "192.168.1.110";
            AvatarInPort = 64101;
            Un8yBlazorPort = 64103;
            NeurULServer = "fibona.cc";
        }
    }

    public class SshConfiguration
    {
        public int ServerAliveInterval { get; set; }
        public int ServerAliveCountMax { get; set; }
        public int Port { get; set; }
        public string HostName { get; set; }
        public string RemoteForward { get; set; }
        public int LocalPort { get; set; }

        /// <summary>
        /// Initialize with defaults
        /// </summary>
        public SshConfiguration(string avatarName)
        {
            ServerAliveInterval = 60;            // 1 minute
            ServerAliveCountMax = 60 * 24 * 365; // 60 secs * 24 hours * 365 days = 1 year
            Port = 2222;
            HostName = "ei8.host";
            RemoteForward = $"{avatarName}:80";
            LocalPort = 9393;
        }

        [JsonConstructor]
        public SshConfiguration() : this("sample") { }
    }
}
