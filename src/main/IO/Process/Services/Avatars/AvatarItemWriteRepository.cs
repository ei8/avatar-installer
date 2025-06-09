using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Cortex.Coding;
using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.neurULization.Persistence;
using ei8.Cortex.Coding.Persistence;
using ei8.EventSourcing.Application;
using ei8.EventSourcing.Client;
using ei8.Extensions.DependencyInjection;
using ei8.Extensions.DependencyInjection.Coding.d23.neurULization.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nancy.TinyIoc;
using System.Reflection;

namespace ei8.Avatar.Installer.IO.Process.Services.Avatars
{
    public class AvatarItemWriteRepository : IAvatarItemWriteRepository
    {
        private readonly IEnumerable<MirrorConfig> mirrorConfigs;
        private readonly ILogger<AvatarItemWriteRepository> logger;

        public AvatarItemWriteRepository(
            IEnumerable<MirrorConfig> mirrorConfigs,
            ILogger<AvatarItemWriteRepository> logger
        )
        {
            this.mirrorConfigs = mirrorConfigs;
            this.logger = logger;
        }

        private List<string> SerializeEnvironmentVariables<T>(T settings)
            where T : class, new()
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
            variablesLines.AddRange(SerializeEnvironmentVariables(avatarItem.Settings.EventSourcing));
            variablesLines.AddRange(SerializeEnvironmentVariables(avatarItem.Settings.CortexGraph));
            variablesLines.AddRange(SerializeEnvironmentVariables(avatarItem.Settings.AvatarApi));
            variablesLines.AddRange(SerializeEnvironmentVariables(avatarItem.Settings.IdentityAccess));
            variablesLines.AddRange(SerializeEnvironmentVariables(avatarItem.Settings.CortexLibrary));
            variablesLines.AddRange(SerializeEnvironmentVariables(avatarItem.Settings.CortexDiaryNucleus));
            await File.WriteAllLinesAsync(Path.Combine(avatarItem.Id, "variables.env"), variablesLines);

            logger.LogInformation("Serializing un8y/variables.env");
            var d23Lines = SerializeEnvironmentVariables(avatarItem.d23);
            await File.WriteAllLinesAsync(Path.Combine(avatarItem.Id, Common.Constants.Directories.Un8y, Common.Constants.Filenames.VariablesEnv), d23Lines);

            logger.LogInformation("Serializing .env");
            var envLines = SerializeEnvironmentVariables(avatarItem.Network);
            await File.WriteAllLinesAsync(Path.Combine(avatarItem.Id, ".env"), envLines);
        }

        private async Task CreateSqliteDatabasesAsync(AvatarItem avatarItem)
        {
            // Save the original directory for later use
            // Maui's current directory: "C:\\WINDOWS\\system32"
            // CLI's current directory: "avatar-installer\\src\\main\\Port.Adapter\\UI\\CLI\\bin\\Debug\\net6.0"
            var originalDirectory = Directory.GetCurrentDirectory();

            // Get the directory where the compiled executable (.exe) is located to fix directory issues
            var exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Directory.SetCurrentDirectory(exeDirectory);

            foreach (var sqlFile in Directory.EnumerateFiles("./Avatars", "*.sql"))
            {
                logger.LogInformation("Creating database for {sqlFile}", sqlFile);

                var sqlStatements = File.ReadAllText(sqlFile);
                var sqliteFileName = $"{Path.GetFileNameWithoutExtension(sqlFile)}.db";

                string sqliteFullPath = Path.Combine(avatarItem.Id, sqliteFileName);
                using var connection = new SqliteConnection($@"Data Source=file:{sqliteFullPath}");

                connection.Open();
                using var command = new SqliteCommand(sqlStatements, connection);
                await command.ExecuteNonQueryAsync();

                if (sqliteFileName == "events.db")
                {
                    var container = new TinyIoCContainer();
                    container.Register(Options.Create(this.mirrorConfigs.ToList()));
                    container.Register<IMirrorRepository, InitializingMirrorRepository>();
                    container.AddInProcessTransactions();
                    container.AddDataAdapters();
                    container.Register<INetworkTransactionData, NetworkTransactionData>();
                    container.Register<INetworkTransactionService, TempNetworkTransactionService>();

                    // TODO: builder.Services.AddScoped<Id23neurULizerOptions, neurULizerOptions>();
                    //builder.Services.AddScoped<IneurULizer, neurULizer>();
                    //builder.Services.AddScoped<IAvatarWriteRepository, AvatarWriteRepository>();


                    container.Resolve<ISettingsService>().DatabasePath = sqliteFullPath;

                    // initialize author neuron
                    var nn = new Network();
                    var authorNeuronId = Guid.NewGuid();
                    nn.AddReplace(Neuron.CreateTransient(
                        authorNeuronId,
                        avatarItem.OwnerName,
                        null,
                    null
                    ));
                    var tr = container.Resolve<ITransaction>();
                    await tr.BeginAsync(authorNeuronId);
                    await container.Resolve<INetworkTransactionService>().SaveAsync(tr, nn);

                    IEnumerable<object> initMirrorKeys = GetAppMirrorKeys();
                    // initialize mirrors
                    var mr = container.Resolve<IMirrorRepository>();
                    var missingInitMirrorConfigs = await mr.GetAllMissingAsync(initMirrorKeys);
                    await mr.Initialize(missingInitMirrorConfigs.Select(mimc => mimc.Key));

                    // initialize avatar
                    // TODO: await this.avatarWriteRepository.Save(new Domain.Model.Avatars.Avatar()
                    //{
                    //    Id = Guid.NewGuid(),
                    //    Name = avatarItem.OwnerName
                    //});

                    await tr.CommitAsync();
                }
            }
            // Revert changes to current directory
            Directory.SetCurrentDirectory(originalDirectory);
        }

        private static IEnumerable<object> GetAppMirrorKeys() =>
            typeof(MirrorSet).GetProperties().Select(p => p.Name).Cast<object>()
                .Concat(new[] {
                    typeof(string),
                    typeof(Guid),
                    typeof(DateTimeOffset)
                });
    }
}
