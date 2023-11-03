using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Domain.Model.Configuration;

namespace ei8.Avatar.Installer.Domain.Model.Mapping
{
    public class AvatarServerMapperService : IAvatarServerMapperService
    {
        // TODO: Add unit tests
        public AvatarServer? Apply(AvatarConfiguration configuration, AvatarServer? item)
        {
            if (item == null)
                return null;

            item.TraefikSettings = ApplyTraefikSettings(configuration, item.TraefikSettings);
            item.SshSettings = ApplySshSettings(configuration.Network.Ssh, item.SshSettings);

            return item;
        }

        private TraefikSettings ApplyTraefikSettings(AvatarConfiguration configuration, TraefikSettings? item) 
        {
            if (item == null)
            {
                item = new TraefikSettings();
            }

            // default values that currently don't have configuration mappings
            item.DefaultEntryPoints = new[] { "http" };
            item.Log = new TraefikLogSettings()
            {
                Level = "DEBUG"
            };
            item.Web = new TraefikWebSettings()
            {
                Address = ":8080"
            };
            item.EntryPoints = new TraefikEntryPointsSettings()
            {
                Http = new TraefikEntryPoint()
                {
                    Address = ":9393"
                }
            };
            item.File = new();

            if (configuration.Avatars?.Any() == true)
            {
                item.Frontends = new();
                item.Backends = new();

                foreach (var avatar in configuration.Avatars)
                {
                    var avatarRouteName = $"{avatar.Name}avatar";
                    var avatarLocalUrl = $"http://{configuration.Network.LocalIp}:{avatar.Network.AvatarInPort}";
                    item.AddRoute(avatarRouteName, $"PathPrefixStrip:/{avatar.Network.NeurULServer}/{avatar.Name}", avatarLocalUrl);

                    var d23RouteName = $"{avatar.Name}d23";
                    var d23LocalUrl = $"http://{configuration.Network.LocalIp}:{avatar.Network.d23BlazorPort}";
                    item.AddRoute(d23RouteName, $"PathPrefixStrip:/{avatar.Network.NeurULServer}/{avatar.Name}/d23", d23LocalUrl);
                }
            }

            return item;
        }

        private SshSettings ApplySshSettings(SshConfiguration configuration, SshSettings? item)
        {
            // revisit if we need to create multiple SSH host settings
            if (item == null)
                item = new();

            item.Hosts = new()
            {
                { "*", new()
                    {
                        HostName = configuration.HostName,
                        RemoteForward = configuration.RemoteForward,
                        ServerAliveCountMax = configuration.ServerAliveCountMax,
                        ServerAliveInterval = configuration.ServerAliveInterval,
                        Port = configuration.Port,
                    }
                }
            };

            return item;
        }
    }
}
