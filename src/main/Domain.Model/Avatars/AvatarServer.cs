using System;
using System.Data;
using System.Reflection.Emit;

namespace ei8.Avatar.Installer.Domain.Model.Avatars
{
    /// <summary>
    /// Represents the network configuration within the user's filesystem
    /// </summary>
    public class AvatarServer
    {
        public TraefikSettings? TraefikSettings { get; set; } = new();
        public SshSettings? SshSettings { get; set; } = new();
    }

    #region Traefik TOML structure

    public class TraefikSettings
    {
        public string[] DefaultEntryPoints { get; set; }
        public TraefikLogSettings Log { get; set; }
        public TraefikWebSettings Web { get; set; }
        public TraefikEntryPointsSettings EntryPoints { get; set; }

        /// <summary>
        /// Marker property that tells Traefik to read the configuration backend from the same TOML file
        /// </summary>
        /// <remarks>
        /// The dictionary is expected to be empty, but it should not be null.
        /// </remarks>
        public Dictionary<string, object?> File { get; set; }
        public Dictionary<string, TraefikFrontend> Frontends { get; set; }
        public Dictionary<string, TraefikBackend> Backends { get; set; }

        public void AddRoute(string name, string frontendRule, string backendUrl, string[]? entryPoints = null)
        {
            entryPoints ??= new[] { "http" };

            if (this.Frontends == null)
                this.Frontends = new();

            this.Frontends.Add(name, new()
            {
                Backend = name,
                Entrypoints = entryPoints,
                Routes = new()
                {
                    { "test_1", new()
                    {
                            Rule = frontendRule
                        }
                    }
                }
            });

            if (this.Backends == null)
                this.Backends = new();

            this.Backends.Add(name, new()
            {
                Servers = new()
                {
                    { "server1", new()
                    {
                            Url = backendUrl
                        }
                    }
                }
            });
        }
    }

    public class TraefikLogSettings
    {
        public string Level { get; set; }
    }

    public class TraefikWebSettings
    {
        public string Address { get; set; }
    }

    public class TraefikEntryPointsSettings
    {
        public TraefikEntryPoint Http { get; set; }
    }

    public class TraefikEntryPoint
    {
        public string Address { get; set; }
    }

    public class TraefikFrontend
    {
        public string Backend { get; set; }
        public string[] Entrypoints { get; set; }
        public Dictionary<string, TraefikFrontendRule> Routes { get; set; }
    }

    public class TraefikFrontendRule
    {
        public string Rule { get; set; }
    }

    public class TraefikBackend
    {
        public Dictionary<string, TraefikBackendServer> Servers { get; set; }
    }

    public class TraefikBackendServer
    {
        public string Url { get; set; }
    }
    #endregion

    public class SshSettings
    {
        public Dictionary<string, SshHostSettings> Hosts { get; set; }
    }

    public class SshHostSettings
    {
        public int ServerAliveInterval { get; set; }
        public int ServerAliveCountMax { get; set; }
        public int Port { get; set; }
        public string HostName { get; set; }
        public string RemoteForward { get; set; }
    }
}
