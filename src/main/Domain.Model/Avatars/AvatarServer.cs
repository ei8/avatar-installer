namespace ei8.Avatar.Installer.Domain.Model.Avatars
{
    /// <summary>
    /// Represents the network configuration within the user's filesystem
    /// </summary>
    public class AvatarServer
    {
        public TraefikSettings TraefikSettings { get; set; } = new();
        public SshSettings SshSettings { get; set; } = new();
    }

    public class TraefikSettings
    {
        public string LogLevel { get; set; }
        public string WebAddress { get; set; }
        public string EntryPointAddress { get; set; }
        public List<TraefikFrontend> Frontends { get; set; } = new();
        public List<TraefikBackend> Backends { get; set; } = new();
    }

    public class TraefikFrontend
    {
        public string Name { get; set; }
        public string BackendName { get; set; }
        public string EntryPointName { get; set; }
        public string EntryPointRule { get; set; }
    }

    public class TraefikBackend
    {
        public string Name { get; set; }
        public string ServerName { get; set; }
        public string Url { get; set; }
    }

    public class SshSettings
    {
        public int ServerAliveInterval { get; set; }
        public int ServerAliveCountMax { get; set; }
        public int Port { get; set; }
        public string HostName { get; set; }
        public string RemoteForward { get; set; }
    }
}
