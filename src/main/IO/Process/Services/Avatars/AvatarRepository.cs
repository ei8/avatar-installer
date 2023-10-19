using System.ComponentModel;
using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.Avatars;
using Microsoft.Extensions.Logging;

namespace ei8.Avatar.Installer.IO.Process.Services.Avatars
{
    public class AvatarRepository : IAvatarRepository
    {
        private readonly ILogger<AvatarRepository> logger;

        public AvatarRepository(ILogger<AvatarRepository> logger) 
        {
            this.logger = logger;
        }

        // TODO: Add unit tests
        public async Task<AvatarItem?> GetByAsync(string id)
        {
            if (!Directory.Exists(id))
            {
                Console.WriteLine($"No files found in {id}");
                return null;
            }

            // assuming id is a path string, get the avatar name from it
            var avatarName = new DirectoryInfo(id).Name;
            var avatarItem = new AvatarItem(avatarName);

            foreach (var file in Directory.EnumerateFiles(id))
            {
                switch (Path.GetFileName(file))
                {
                    // load other files here as needed
                    case "variables.env":
                        {
                            logger.LogInformation($"Loading {file}");

                            var variables = await GetEnvironmentVariablesFromFileAsync(file);
                            avatarItem.CortexGraph = CreateFromEnvironmentVariables<CortexGraphSettings>(variables);
                            avatarItem.EventSourcing = CreateFromEnvironmentVariables<EventSourcingSettings>(variables);
                            avatarItem.AvatarApi = CreateFromEnvironmentVariables<AvatarApiSettings>(variables);
                            avatarItem.IdentityAccess = CreateFromEnvironmentVariables<IdentityAccessSettings>(variables);
                            avatarItem.CortexLibrary = CreateFromEnvironmentVariables<CortexLibrarySettings>(variables);
                            avatarItem.CortexDiaryNucleus = CreateFromEnvironmentVariables<CortexDiaryNucleusSettings>(variables);
                        }
                        break;

                    case ".env":
                        {
                            logger.LogInformation($"Loading {file}");

                            var variables = await GetEnvironmentVariablesFromFileAsync(file);
                            avatarItem.Network = CreateFromEnvironmentVariables<AvatarNetworkSettings>(variables);
                        }
                        break;

                    case "d23-variables.env":
                        {
                            logger.LogInformation($"Loading {file}");

                            var variables = await GetEnvironmentVariablesFromFileAsync(file);
                            avatarItem.D23 = CreateFromEnvironmentVariables<D23Settings>(variables);
                        }
                        break;
                }
            }

            return avatarItem;
        }

        private async Task<Dictionary<string, string>> GetEnvironmentVariablesFromFileAsync(string file)
        {
            return (await File.ReadAllLinesAsync(file))
                              .Where(l => !l.StartsWith('#') && !string.IsNullOrWhiteSpace(l)) // ignore comments and newlines
                              .Select(l => l.Split('='))                                       // split into variable, value
                              .ToDictionary(l => l[0], l => l[1]);
        }

        private T? CreateFromEnvironmentVariables<T>(Dictionary<string, string> variables)
            where T : class, new()
        {
            if (!variables.Any())
                return null;

            var settings = new T();

            foreach (var property in settings.GetType().GetProperties())
            {
                var environmentVariableKey = property.Name.ToMacroCase();

                if (variables.TryGetValue(environmentVariableKey, out var savedValue))
                {
                    if (property.PropertyType == typeof(string))
                    {
                        property.SetValue(settings, savedValue);
                        continue;
                    }

                    var converter = TypeDescriptor.GetConverter(property.PropertyType);

                    if (converter != null)
                    {
                        var convertedValue = converter.ConvertFrom(savedValue);
                        property.SetValue(settings, convertedValue);
                    }
                }
            }

            return settings;
        }

        public Task SaveAsync(AvatarItem avatarItem)
        {
            // TODO: commit AvatarItem to filesystem

            // TODO: generate the following files
            // sql scripts for sqlite databases (save and execute to create dbs)
            throw new NotImplementedException();
        }
    }
}
