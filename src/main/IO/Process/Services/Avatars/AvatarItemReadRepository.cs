using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Domain.Model.Avatars.Settings;
using Microsoft.Extensions.Logging;

namespace ei8.Avatar.Installer.IO.Process.Services.Avatars
{
    public class AvatarItemReadRepository : IAvatarItemReadRepository
    {
        private readonly ILogger<AvatarItemWriteRepository> logger;

        public AvatarItemReadRepository(
            ILogger<AvatarItemWriteRepository> logger
        )
        {
            this.logger = logger;
        }

        // TODO: Add unit tests
        public async Task<AvatarItem> GetByAsync(string id)
        {
            if (!Directory.Exists(id))
            {
                logger.LogInformation("No files found in {id}", id);
                return null;
            }

            // assuming id is a path string, get the avatar name from it
            var avatarName = new DirectoryInfo(id).Name;
            var avatarItem = new AvatarItem(id, avatarName);

            foreach (var file in Directory.EnumerateFiles(id))
            {
                switch (Path.GetFileName(file))
                {
                    // load other files here as needed
                    case Common.Constants.Filenames.VariablesEnv:
                        logger.LogInformation("Loading {file}", file);

                        var variables = await GetEnvironmentVariablesFromFileAsync(file);
                        avatarItem.Settings.CortexGraph = DeserializeEnvironmentVariables<CortexGraphSettings>(variables);
                        avatarItem.Settings.EventSourcing = DeserializeEnvironmentVariables<EventSourcingSettings>(variables);
                        avatarItem.Settings.AvatarApi = DeserializeEnvironmentVariables<AvatarApiSettings>(variables);
                        avatarItem.Settings.IdentityAccess = DeserializeEnvironmentVariables<IdentityAccessSettings>(variables);
                        avatarItem.Settings.CortexLibrary = DeserializeEnvironmentVariables<CortexLibrarySettings>(variables);
                        avatarItem.Settings.CortexDiaryNucleus = DeserializeEnvironmentVariables<CortexDiaryNucleusSettings>(variables);
                        break;

                    case ".env":
                        logger.LogInformation("Loading {file}", file);

                        avatarItem.Network = DeserializeEnvironmentVariables<AvatarNetworkSettings>(
                            await GetEnvironmentVariablesFromFileAsync(file)
                        );
                        break;
                }
            }

            // un8y
            foreach (var file in Directory.EnumerateFiles(id + "/" + Common.Constants.Directories.Un8y))
            {
                switch (Path.GetFileName(file))
                {
                    case Common.Constants.Filenames.VariablesEnv:
                        logger.LogInformation("Loading {file}", file);

                        var variables = await GetEnvironmentVariablesFromFileAsync(file);
                        avatarItem.d23 = DeserializeEnvironmentVariables<d23Settings>(variables);
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

        private T DeserializeEnvironmentVariables<T>(Dictionary<string, string> variables)
            where T : class, new()
        {
            if (!variables.Any())
                return null;

            var settings = new T();

            foreach (var property in settings.GetType().GetProperties())
            {
                var environmentVariableKey = property.Name.ToMacroCase();

                if (variables.TryGetValue(environmentVariableKey, out var savedValue))
                    property.SetValueFromString(settings, savedValue);
            }

            return settings;
        }

    }
}
