using System.Text;
using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.Avatars;
using Microsoft.Extensions.Logging;
using Tomlyn;

namespace ei8.Avatar.Installer.IO.Process.Services.Avatars
{
    public class AvatarServerRepository : IAvatarServerRepository
    {
        private readonly ILogger<AvatarServerRepository> logger;

        public AvatarServerRepository(ILogger<AvatarServerRepository> logger)
        {
            this.logger = logger;
        }

        // TODO: add unit tests
        public async Task<AvatarServer?> GetByAsync(string id)
        {
            if (!Directory.Exists(id))
            {
                logger.LogWarning("Unable to find directory {id}", id);
                return null;
            }

            var result = new AvatarServer()
            {
                Id = id,
                TraefikSettings = await DeserializeTraefikFile(Path.Combine(id, Constants.Filenames.TraefikToml)),
                SshSettings = await DeserializeSshSettingsFile(Path.Combine(id, Constants.Filenames.SshConfig)),
            };

            return result;
        }

        public async Task SaveAsync(AvatarServer avatarServer)
        {
            if (!Directory.Exists(avatarServer.Id))
            {
                logger.LogWarning("Unable to find directory {id}", avatarServer.Id);
                return;
            }

            await SerializeTraefikFileAsync(Path.Combine(avatarServer.Id, Constants.Filenames.TraefikToml), avatarServer.TraefikSettings!);
            await SerializeSshSettingsFileAsync(Path.Combine(avatarServer.Id, Constants.Filenames.SshConfig), avatarServer.SshSettings!);

            await CreateBatchFilesAsync(avatarServer.Id);
        }

        private async Task CreateBatchFilesAsync(string path)
        {
            // start - traefik.bat
            logger.LogInformation("Creating {fileName}", Constants.Filenames.StartTraefikBat);
            var startTraefikBatPath = Path.Combine(path, Constants.Filenames.StartTraefikBat);
            var traefikScript = String.Format(Constants.BatchFileTemplates.StartTraefikBat, Constants.Filenames.TraefikToml);
            await File.WriteAllTextAsync(startTraefikBatPath, traefikScript);

            // start - ei8.site.bat
            logger.LogInformation("Creating {fileName}", Constants.Filenames.StartEi8SiteBat);
            var startEi8SiteBatPath = Path.Combine(path, Constants.Filenames.StartEi8SiteBat);
            var sshScript = String.Format(Constants.BatchFileTemplates.StartEi8SiteBat, Path.GetFullPath(Path.Combine(path, Constants.Filenames.SshConfig)));
            await File.WriteAllTextAsync(startEi8SiteBatPath, sshScript);

            // loop start - ei8.site.bat
            logger.LogInformation("Creating {fileName}", Constants.Filenames.LoopStartEi8SiteBat);
            var loopStartEi8SiteBatPath = Path.Combine(path, Constants.Filenames.LoopStartEi8SiteBat);
            await File.WriteAllTextAsync(loopStartEi8SiteBatPath, Constants.BatchFileTemplates.LoopStartEi8SiteBat);

            // autolock.cmd
            logger.LogInformation("Creating {fileName}", Constants.Filenames.AutoLockCmd);
            var autolockCmdPath = Path.Combine(path, Constants.Filenames.AutoLockCmd);
            await File.WriteAllTextAsync(autolockCmdPath, Constants.BatchFileTemplates.AutoLockCmd);
        }

        private async Task<SshSettings?> DeserializeSshSettingsFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                logger.LogWarning("Unable to find SSH settings file: {fileName}", fileName);
                return null;
            }

            logger.LogInformation("Deserializing {fileName}", fileName);

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

        private async Task SerializeSshSettingsFileAsync(string fileName, SshSettings settings)
        {
            logger.LogInformation("Creating {fileName}", fileName);

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
            {
                logger.LogWarning("Unable to find traefik TOML file: {fileName}", fileName);
                return null;
            }

            logger.LogInformation("Deserializing {fileName}", fileName);

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

        private async Task SerializeTraefikFileAsync(string fileName, TraefikSettings settings)
        {
            logger.LogInformation("Creating {fileName}", fileName);

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
