using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.Avatars;
using Microsoft.Data.Sqlite;
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
                    case "variables.env":
                        {
                            logger.LogInformation("Loading {file}", file);

                            var variables = await GetEnvironmentVariablesFromFileAsync(file);
                            avatarItem.CortexGraph = DeserializeEnvironmentVariables<CortexGraphSettings>(variables);
                            avatarItem.EventSourcing = DeserializeEnvironmentVariables<EventSourcingSettings>(variables);
                            avatarItem.AvatarApi = DeserializeEnvironmentVariables<AvatarApiSettings>(variables);
                            avatarItem.IdentityAccess = DeserializeEnvironmentVariables<IdentityAccessSettings>(variables);
                            avatarItem.CortexLibrary = DeserializeEnvironmentVariables<CortexLibrarySettings>(variables);
                            avatarItem.CortexDiaryNucleus = DeserializeEnvironmentVariables<CortexDiaryNucleusSettings>(variables);
                        }
                        break;

                    case ".env":
                        {
                            logger.LogInformation("Loading {file}", file);

                            var variables = await GetEnvironmentVariablesFromFileAsync(file);
                            avatarItem.Network = DeserializeEnvironmentVariables<AvatarNetworkSettings>(variables);
                        }
                        break;

                    case "d23-variables.env":
                        {
                            logger.LogInformation("Loading {file}", file);

                            var variables = await GetEnvironmentVariablesFromFileAsync(file);
                            avatarItem.d23 = DeserializeEnvironmentVariables<d23Settings>(variables);
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

        private T? DeserializeEnvironmentVariables<T>(Dictionary<string, string> variables)
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

        private List<string>? SerializeEnvironmentVariables<T>(T settings)
            where T: class, new()
        {
            if (settings == null)
                return null;

            var lines = new List<string>();

            foreach (var property in settings.GetType().GetProperties())
            {
                var environmentVariableKey = property.Name.ToMacroCase();
                var environmentVariableValue = property.GetValue(settings);

                if (property == typeof(bool))
                    environmentVariableValue = ((string)environmentVariableValue).ToLower();

                if (environmentVariableValue != null)
                {
                    lines.Add($"{environmentVariableKey}={environmentVariableValue}");
                }
            }

            return lines;
        }

        public async Task SaveAsync(AvatarItem avatarItem)
        {
            await SaveEnvironmentVariablesAsync(avatarItem);
            await CreateSqliteDatabasesAsync(avatarItem.Id);
        }

        private async Task SaveEnvironmentVariablesAsync(AvatarItem avatarItem)
        {
            logger.LogInformation("Serializing variables.env");
            var variablesLines = new List<string>();
            variablesLines.AddRange(SerializeEnvironmentVariables(avatarItem.EventSourcing));
            variablesLines.AddRange(SerializeEnvironmentVariables(avatarItem.CortexGraph));
            variablesLines.AddRange(SerializeEnvironmentVariables(avatarItem.AvatarApi));
            variablesLines.AddRange(SerializeEnvironmentVariables(avatarItem.IdentityAccess));
            variablesLines.AddRange(SerializeEnvironmentVariables(avatarItem.CortexLibrary));
            variablesLines.AddRange(SerializeEnvironmentVariables(avatarItem.CortexDiaryNucleus));
            await File.WriteAllLinesAsync(Path.Combine(avatarItem.Id, "variables.env"), variablesLines);

            logger.LogInformation("Serializing d23-variables.env");
            var d23Lines = SerializeEnvironmentVariables(avatarItem.d23);
            await File.WriteAllLinesAsync(Path.Combine(avatarItem.Id, "d23-variables.env"), d23Lines);

            logger.LogInformation("Serializing .env");
            var envLines = SerializeEnvironmentVariables(avatarItem.Network);
            await File.WriteAllLinesAsync(Path.Combine(avatarItem.Id, ".env"), envLines);
        }

        private async Task CreateSqliteDatabasesAsync(string id)
        {
            foreach (var sqlFile in Directory.EnumerateFiles("./Avatars", "*.sql"))
            {
                logger.LogInformation("Creating database for {sqlFile}", sqlFile);

                var sqlStatements = File.ReadAllText(sqlFile);
                var sqliteFileName = $"{Path.GetFileNameWithoutExtension(sqlFile)}.db";

                using (var connection = new SqliteConnection($@"Data Source=file:{Path.Combine(id, sqliteFileName)}"))
                {
                    connection.Open();

                    using var cmd = new SqliteCommand(sqlStatements, connection);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
