using ei8.Avatar.Installer.Domain.Model.DTO;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.IO.Process.Services.IdentityAccess;

public class RegionPermitRepository : IRegionPermitRepository
{
    public async Task<IEnumerable<RegionPermit>> GetRegionPermitsAsync(string access)
    {
        var connectionString = $@"Data Source=file:{Path.Combine(access, "identity-access.db")}";
        var regionPermits = new List<RegionPermit>();
        var tableName = "RegionPermit";

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        using var command = new SqliteCommand($"SELECT SequenceId, UserNeuronId, RegionNeuronId, WriteLevel, ReadLevel FROM {tableName}", connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var regionPermit = new RegionPermit(
                 reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                 reader.IsDBNull(1) ? null : reader.GetString(1),
                 reader.IsDBNull(2) ? null : reader.GetString(2),
                 reader.IsDBNull(3) ? null : reader.GetInt32(3),
                 reader.IsDBNull(4) ? null : reader.GetInt32(4));

            regionPermits.Add(regionPermit);
        }

        return regionPermits;
    }

    public async Task UpdateRegionPermitAsync(string access, RegionPermit regionPermit)
    {
        var connectionString = $@"Data Source=file:{Path.Combine(access, "identity-access.db")}";
        var tableName = "RegionPermit";

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var query = $"UPDATE {tableName} SET UserNeuronId = @UserNeuronId, RegionNeuronId = @RegionNeuronId, WriteLevel = @WriteLevel, ReadLevel = @ReadLevel WHERE SequenceId = @SequenceId";
        using var command = new SqliteCommand(query, connection);

        command.Parameters.AddWithValue("@UserNeuronId", string.IsNullOrEmpty(regionPermit.UserNeuronId) ? DBNull.Value : regionPermit.UserNeuronId);
        command.Parameters.AddWithValue("@RegionNeuronId", string.IsNullOrEmpty(regionPermit.RegionNeuronId) ? DBNull.Value : regionPermit.RegionNeuronId);
        command.Parameters.AddWithValue("@WriteLevel", regionPermit.WriteLevel is null ? DBNull.Value : regionPermit.WriteLevel);
        command.Parameters.AddWithValue("@ReadLevel", regionPermit.ReadLevel is null ? DBNull.Value : regionPermit.ReadLevel);

        command.Parameters.AddWithValue("@SequenceId", regionPermit.SequenceId);

        await command.ExecuteNonQueryAsync();
    }
}
