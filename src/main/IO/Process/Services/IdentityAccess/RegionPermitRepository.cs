using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using Microsoft.Data.Sqlite;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.IO.Process.Services.IdentityAccess;

public class RegionPermitRepository : IRegionPermitRepository
{
    private readonly IAvatarContextService avatarContextService;

    public RegionPermitRepository(IAvatarContextService avatarContextService)
    {
        AssertionConcern.AssertArgumentNotNull(avatarContextService, nameof(avatarContextService));

        this.avatarContextService = avatarContextService;
    }

    public async Task<RegionPermit> GetByIdAsync(int sequenceId)
    {
        var id = avatarContextService.Avatar.Id;
        var connectionString = $@"Data Source=file:{Path.Combine(id, Constants.Databases.IdentityAccessDb)}";

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var query = $"SELECT SequenceId, UserNeuronId, RegionNeuronId, WriteLevel, ReadLevel FROM {Constants.TableNames.RegionPermit} WHERE SequenceId = @SequenceId";
        using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@SequenceId", sequenceId);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var regionPermit = new RegionPermit()
            {
                SequenceId = reader.GetInt32(0),
                UserNeuronId = reader.IsDBNull(1) ? null : reader.GetString(1),
                RegionNeuronId = reader.IsDBNull(2) ? null : reader.GetString(2),
                WriteLevel = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                ReadLevel = reader.IsDBNull(4) ? null : reader.GetInt32(4)
            };
            return regionPermit;
        }

        return null;
    }

    public async Task<IEnumerable<RegionPermit>> GetAllAsync()
    {
        var id = avatarContextService.Avatar.Id;
        var connectionString = $@"Data Source=file:{Path.Combine(id, Constants.Databases.IdentityAccessDb)}";
        var regionPermits = new List<RegionPermit>();

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        using var command = new SqliteCommand($"SELECT SequenceId, UserNeuronId, RegionNeuronId, WriteLevel, ReadLevel FROM {Constants.TableNames.RegionPermit}", connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var regionPermit = new RegionPermit
            {
                SequenceId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                UserNeuronId = reader.IsDBNull(1) ? null : reader.GetString(1),
                RegionNeuronId = reader.IsDBNull(2) ? null : reader.GetString(2),
                WriteLevel = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                ReadLevel = reader.IsDBNull(4) ? null : reader.GetInt32(4)
            };

            regionPermits.Add(regionPermit);
        }

        return regionPermits;
    }

    public async Task RemoveAsync(RegionPermit regionPermit)
    {
        AssertionConcern.AssertArgumentNotNull(regionPermit, nameof(regionPermit));

        var id = avatarContextService.Avatar.Id;
        var connectionString = $@"Data Source=file:{Path.Combine(id, Constants.Databases.IdentityAccessDb)}";

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var query = $"DELETE FROM {Constants.TableNames.RegionPermit} WHERE SequenceId = @SequenceId";
        using var command = new SqliteCommand(query, connection);

        command.Parameters.AddWithValue("@SequenceId", regionPermit.SequenceId);

        await command.ExecuteNonQueryAsync();
    }

    public async Task SaveAsync(RegionPermit regionPermit)
    {
        AssertionConcern.AssertArgumentNotNull(regionPermit, nameof(regionPermit));

        var id = avatarContextService.Avatar.Id;
        var connectionString = $@"Data Source=file:{Path.Combine(id, Constants.Databases.IdentityAccessDb)}";

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        string query;
        if (regionPermit.SequenceId <= 0)
            query = $@"
                INSERT INTO {Constants.TableNames.RegionPermit} (UserNeuronId, RegionNeuronId, WriteLevel, ReadLevel)
                VALUES (@UserNeuronId, @RegionNeuronId, @WriteLevel, @ReadLevel)";
        else
            query = $@"
                INSERT OR REPLACE INTO {Constants.TableNames.RegionPermit} (SequenceId, UserNeuronId, RegionNeuronId, WriteLevel, ReadLevel)
                VALUES (@SequenceId, @UserNeuronId, @RegionNeuronId, @WriteLevel, @ReadLevel)";
        using var command = new SqliteCommand(query, connection);

        command.Parameters.AddWithValue("@SequenceId", regionPermit.SequenceId);
        command.Parameters.AddWithValue("@UserNeuronId", string.IsNullOrEmpty(regionPermit.UserNeuronId) ? DBNull.Value : regionPermit.UserNeuronId);
        command.Parameters.AddWithValue("@RegionNeuronId", string.IsNullOrEmpty(regionPermit.RegionNeuronId) ? DBNull.Value : regionPermit.RegionNeuronId);
        command.Parameters.AddWithValue("@WriteLevel", regionPermit.WriteLevel is null ? DBNull.Value : regionPermit.WriteLevel);
        command.Parameters.AddWithValue("@ReadLevel", regionPermit.ReadLevel is null ? DBNull.Value : regionPermit.ReadLevel);

        await command.ExecuteNonQueryAsync();
    }
}
