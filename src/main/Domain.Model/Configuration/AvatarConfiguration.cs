using System.Runtime.Intrinsics.X86;
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
        public SharedNetworkConfiguration Network { get; set; } = new();
    }

    public class SharedNetworkConfiguration
    {
        public string LocalIp { get; set; }
        public SshConfiguration Ssh { get; set; } = new();

        /// <summary>
        /// Initialize with defaults
        /// </summary>
        public SharedNetworkConfiguration(string avatarName)
        {
            LocalIp = "192.168.1.110";
            Ssh = new(avatarName);
        }

        [JsonConstructor]
        public SharedNetworkConfiguration() : this("sample") { }
    }

    public class AvatarConfigurationItem
    {
        public string Name { get; set; }
        public CortexGraphConfiguration CortexGraph { get; set; } = new();
        public AvatarApiConfiguration AvatarApi { get; set; } = new();
        public CortexLibraryConfiguration CortexLibrary { get; set; } = new();
        public d23Configuration D23 { get; set; } = new();
        public NetworkConfiguration Network { get; set; } = new();

        [JsonConstructor]
        public AvatarConfigurationItem(string name)
        {
            Name = name;
            AvatarApi = new(name);
            CortexLibrary = new(name);
            D23 = new(name);
        }
    }

    public class CortexGraphConfiguration
    {
        public string DbName { get; set; }
        public string DbUsername { get; set; }
        public string DbUrl { get; set; }
        public string ArangoRootPassword { get; set; }

        /// <summary>
        /// Initialize with defaults
        /// </summary>
        public CortexGraphConfiguration()
        {
            DbName = "graph";
            DbUsername = "root";
            DbUrl = "http://cortex.graph.persistence:8529";
            ArangoRootPassword = string.Empty;
        }
    }

    public class AvatarApiConfiguration
    {
        public string TokenIssuerUrl { get; set; }
        public string ApiName { get; set; }

        /// <summary>
        /// Initialize with defaults
        /// </summary>
        public AvatarApiConfiguration(string avatarName)
        {
            TokenIssuerUrl = @"https://login.fibona.cc";
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

    public class d23Configuration
    {
        public string OidcAuthorityUrl { get; set; }
        public string ClientId { get; set; }
        public string BasePath { get; set; }

        /// <summary>
        /// Initialize with defaults
        /// </summary>
        public d23Configuration(string avatarName)
        {
            OidcAuthorityUrl = @"https://login.fibona.cc";
            ClientId = $"d23-{avatarName}";
            BasePath = $"/{avatarName}/d23";
        }

        [JsonConstructor]
        public d23Configuration() : this("sample") { }
    }

    public class NetworkConfiguration
    {
        public string LocalIp { get; set; }
        public int AvatarInPort { get; set; }
        public int D23BlazorPort { get; set; }

        /// <summary>
        /// Initialize with defaults
        /// </summary>

        [JsonConstructor]
        public NetworkConfiguration()
        {
            LocalIp = "192.168.1.110";
            AvatarInPort = 64101;
            D23BlazorPort = 64103;
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
