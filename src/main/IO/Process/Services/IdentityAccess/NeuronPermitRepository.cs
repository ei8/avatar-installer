using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using Microsoft.Data.Sqlite;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.IO.Process.Services.IdentityAccess;

public class NeuronPermitRepository : INeuronPermitRepository
{
    private readonly IAvatarContextService avatarContextService;

    public NeuronPermitRepository(IAvatarContextService avatarContextService)
    {
        AssertionConcern.AssertArgumentNotNull(avatarContextService, nameof(avatarContextService));

        this.avatarContextService = avatarContextService;
    }

    public async Task DeleteAsync(NeuronPermit neuronPermit)
    {
        AssertionConcern.AssertArgumentNotNull(neuronPermit, nameof(neuronPermit));

        var id = avatarContextService.Avatar.Id;
        var connectionString = $@"Data Source=file:{Path.Combine(id, Constants.Databases.IdentityAccessDb)}";

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var query = $"DELETE FROM {Constants.TableNames.NeuronPermit} WHERE UserNeuronId = @UserNeuronId AND NeuronId = @NeuronId";
        using var command = new SqliteCommand(query, connection);

        command.Parameters.AddWithValue("@UserNeuronId", neuronPermit.UserNeuronId);
        command.Parameters.AddWithValue("@NeuronId", neuronPermit.NeuronId);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<IEnumerable<NeuronPermit>> GetAllAsync()
    {
        var id = avatarContextService.Avatar.Id;
        var connectionString = $@"Data Source=file:{Path.Combine(id, Constants.Databases.IdentityAccessDb)}";
        var neuronPermits = new List<NeuronPermit>();

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var query = $"SELECT UserNeuronId, NeuronId, ExpirationDate FROM {Constants.TableNames.NeuronPermit}";
        using var command = new SqliteCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var neuronPermit = new NeuronPermit
            {
                UserNeuronId = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                NeuronId = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                ExpirationDate = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
            };

            neuronPermits.Add(neuronPermit);
        }

        return neuronPermits;
    }

    public async Task UpdateAsync(NeuronPermit neuronPermit)
    {
        AssertionConcern.AssertArgumentNotNull(neuronPermit, nameof(neuronPermit));

        var id = avatarContextService.Avatar.Id;
        var connectionString = $@"Data Source=file:{Path.Combine(id, Constants.Databases.IdentityAccessDb)}";

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var query = $"UPDATE {Constants.TableNames.NeuronPermit} SET ExpirationDate = @ExpirationDate WHERE UserNeuronId = @UserNeuronId AND NeuronId = @NeuronId";
        using var command = new SqliteCommand(query, connection);

        command.Parameters.AddWithValue("@ExpirationDate", string.IsNullOrEmpty(neuronPermit.ExpirationDate) ? DBNull.Value : neuronPermit.ExpirationDate);

        command.Parameters.AddWithValue("@UserNeuronId", neuronPermit.UserNeuronId);
        command.Parameters.AddWithValue("@NeuronId", neuronPermit.NeuronId);

        await command.ExecuteNonQueryAsync();
    }
}