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
            await CreateSqliteDatabasesAsync(avatarItem);
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

        private async Task CreateSqliteDatabasesAsync(AvatarItem avatarItem)
        {
            foreach (var sqlFile in Directory.EnumerateFiles("./Avatars", "*.sql"))
            {
                logger.LogInformation("Creating database for {sqlFile}", sqlFile);

                var sqlStatements = File.ReadAllText(sqlFile);
                var sqliteFileName = $"{Path.GetFileNameWithoutExtension(sqlFile)}.db";

                using var connection = new SqliteConnection($@"Data Source=file:{Path.Combine(avatarItem.Id, sqliteFileName)}");

                connection.Open();

                if (sqliteFileName == "events.db")
                {
                    #region Notification Table Insertion

                    string sqlInsertionCommand = @"
BEGIN TRANSACTION;
INSERT OR REPLACE INTO ""Notification"" (""SequenceId"", ""Timestamp"", ""TypeName"", ""Id"", ""Version"", ""AuthorId"", ""Data"")
VALUES
    (@SequenceId1, @Timestamp1, @TypeName1, @Id1, @Version1, @AuthorId1, @Data1),
    (@SequenceId2, @Timestamp2, @TypeName2, @Id2, @Version2, @AuthorId2, @Data2);
COMMIT;";

                    var guid = Guid.NewGuid();
                    var authorGuid = Guid.NewGuid();

                    int sequenceId1 = 1;
                    string timestamp1 = DateTime.Now.ToString();
                    string typeName1 = "neurUL.Cortex.Domain.Model.Neurons.NeuronCreated, neurUL.Cortex.Domain.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                    string id1 = guid.ToString();
                    int version1 = 1;
                    string authorId1 = authorGuid.ToString();
                    string data1 = "{\"Id\":\"" + id1 + "\",\"Version\":" + version1 + ",\"Timestamp\":\"" + timestamp1 + "\"}";

                    int sequenceId2 = 2;
                    string timestamp2 = DateTime.Now.ToString();
                    string typeName2 = "ei8.Data.Tag.Domain.Model.TagChanged, ei8.Data.Tag.Domain.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                    string id2 = guid.ToString();
                    int version2 = 2;
                    string authorId2 = authorGuid.ToString();
                    string data2 = "{\"Tag\":\"" + avatarItem.OwnerName + "\",\"Id\":\"" + id2 + "\",\"Version\":" + version2 + ",\"Timestamp\":\"" + timestamp2 + "\"}";

                    using var command = new SqliteCommand(sqlStatements + sqlInsertionCommand, connection);

                    command.Parameters.AddWithValue("@SequenceId1", sequenceId1);
                    command.Parameters.AddWithValue("@Timestamp1", timestamp1);
                    command.Parameters.AddWithValue("@TypeName1", typeName1);
                    command.Parameters.AddWithValue("@Id1", id1);
                    command.Parameters.AddWithValue("@Version1", version1);
                    command.Parameters.AddWithValue("@AuthorId1", authorId1);
                    command.Parameters.AddWithValue("@Data1", data1);

                    command.Parameters.AddWithValue("@SequenceId2", sequenceId2);
                    command.Parameters.AddWithValue("@Timestamp2", timestamp2);
                    command.Parameters.AddWithValue("@TypeName2", typeName2);
                    command.Parameters.AddWithValue("@Id2", id2);
                    command.Parameters.AddWithValue("@Version2", version2);
                    command.Parameters.AddWithValue("@AuthorId2", authorId2);
                    command.Parameters.AddWithValue("@Data2", data2);

                    await command.ExecuteNonQueryAsync();

                    #endregion
                }
                else
                {
                    using var command = new SqliteCommand(sqlStatements, connection);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
