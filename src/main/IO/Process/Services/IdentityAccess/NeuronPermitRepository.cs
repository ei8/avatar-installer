using ei8.Avatar.Installer.Domain.Model.DTO;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.IO.Process.Services.IdentityAccess;

public class NeuronPermitRepository : INeuronPermitRepository
{
    public async Task<IEnumerable<NeuronPermit>> GetNeuronPermitsAsync(string access)
    {
        var connectionString = $@"Data Source=file:{Path.Combine(access, "identity-access.db")}";
        var neuronPermits = new List<NeuronPermit>();
        var tableName = "NeuronPermit";

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var query = $"SELECT UserNeuronId, NeuronId, ExpirationDate FROM {tableName}";
        using var command = new SqliteCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var neuronPermit = new NeuronPermit(
                 reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                 reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                 reader.IsDBNull(2) ? string.Empty : reader.GetString(2));

            neuronPermits.Add(neuronPermit);
        }

        return neuronPermits;
    }

    public async Task UpdateNeuronPermitAsync(string access, NeuronPermit neuronPermit)
    {
        var connectionString = $@"Data Source=file:{Path.Combine(access, "identity-access.db")}";
        var tableName = "NeuronPermit";

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var query = $"UPDATE {tableName} SET ExpirationDate = @ExpirationDate WHERE UserNeuronId = @UserNeuronId AND NeuronId = @NeuronId";
        using var command = new SqliteCommand(query, connection);

        command.Parameters.AddWithValue("@ExpirationDate", string.IsNullOrEmpty(neuronPermit.ExpirationDate) ? DBNull.Value : neuronPermit.ExpirationDate);

        command.Parameters.AddWithValue("@UserNeuronId", neuronPermit.UserNeuronId);
        command.Parameters.AddWithValue("@NeuronId", neuronPermit.NeuronId);

        await command.ExecuteNonQueryAsync();
    }
}