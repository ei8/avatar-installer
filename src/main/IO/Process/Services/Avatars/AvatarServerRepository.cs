using System.Text;
using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.Avatars;
using Tomlyn;

namespace ei8.Avatar.Installer.IO.Process.Services.Avatars
{
    public class AvatarServerRepository : IAvatarServerRepository
    {
        public async Task<AvatarServer?> GetByAsync(string id)
        {
            if (!Directory.Exists(id))
                return null;

            var result = new AvatarServer();

            result.TraefikSettings = await DeserializeTraefikFile(Path.Combine(id, "traefik.toml"));
            result.SshSettings = await DeserializeSshSettingsFile(Path.Combine(id, "ssh_config"));

            return result;
        }

        public async Task SaveAsync(string id, AvatarServer avatarServer)
        {
            await SerializeTraefikFile(Path.Combine(id, "traefik.toml"), avatarServer.TraefikSettings!);
            await SerializeSshSettingsFile(Path.Combine(id, "ssh_config"), avatarServer.SshSettings!);
        }

        private async Task<SshSettings?> DeserializeSshSettingsFile(string fileName)
        {
            if (!File.Exists(fileName))
                return null;

            var result = new SshSettings()
            {
                Hosts = new()
            };

            using (var file = new StreamReader(fileName))
            {
                string? line;
                string? currentKey = null;

                while ((line = await file.ReadLineAsync()) != null)
                {
                    if (line.StartsWith("Host"))
                    {
                        var hostLine = line.Split(' ');
                        currentKey = hostLine[1];

                        result.Hosts.Add(currentKey, new SshHostSettings());
                    }
                    else if (line.StartsWith('\t') || line.StartsWith("    "))
                    {
                        var settingLine = line.TrimStart()
                                              .Split(' ');

                        var propName = settingLine[0];
                        var propValue = string.Join(' ', settingLine.Skip(1));

                        var prop = typeof(SshHostSettings).GetProperty(propName);

                        if (prop != null)
                            prop.SetValueFromString(result.Hosts[currentKey!], propValue);
                    }
                }
            }

            return result;
        }

        private async Task SerializeSshSettingsFile(string fileName, SshSettings settings)
        {
            var sb = new StringBuilder();

            foreach (var kvp in settings.Hosts)
            {
                sb.AppendLine($"Host {kvp.Key}");

                var props = typeof(SshHostSettings).GetProperties();
                foreach (var prop in props)
                {
                    var propValue = prop.GetValue(kvp.Value);

                    if (propValue != null)
                        sb.AppendLine($"\t{prop.Name} {propValue}");
                }
            }

            await File.WriteAllTextAsync(fileName, sb.ToString());
        }

        private async Task<TraefikSettings?> DeserializeTraefikFile(string fileName)
        {
            if (!File.Exists(fileName))
                return null;

            var tomlString = await File.ReadAllTextAsync(fileName);
            var result = Toml.ToModel<TraefikSettings>(tomlString, options: new TomlModelOptions()
            {
                ConvertPropertyName = (fieldName) =>
                {
                    return string.Concat(char.ToLower(fieldName[0]), fieldName.Substring(1));
                }
            });

            return result;
        }

        private async Task SerializeTraefikFile(string fileName, TraefikSettings settings)
        {
            var tomlString = Toml.FromModel(settings, new TomlModelOptions()
            {
                ConvertPropertyName = (fieldName) =>
                {
                    return string.Concat(char.ToLower(fieldName[0]), fieldName.Substring(1));
                },
            });

            await File.WriteAllTextAsync(fileName, tomlString);
        }
    }
}
