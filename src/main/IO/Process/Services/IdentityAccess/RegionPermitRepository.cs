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
    public async Task<IEnumerable<RegionPermitDto>> GetRegionPermitsAsync(string access)
    {
        var connectionString = $@"Data Source=file:{Path.Combine(access, "identity-access.db")}";
        var regionPermits = new List<RegionPermitDto>();
        var tableName = "RegionPermit";

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        using var command = new SqliteCommand($"SELECT SequenceId, UserNeuronId, RegionNeuronId, WriteLevel, ReadLevel FROM {tableName}", connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var regionPermit = new RegionPermitDto
            {
                SequenceId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                UserNeuronId = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                RegionNeuronId = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                WriteLevel = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                ReadLevel = reader.IsDBNull(4) ? 0 : reader.GetInt32(4)
            };

            regionPermits.Add(regionPermit);
        }

        return regionPermits;
    }
}
