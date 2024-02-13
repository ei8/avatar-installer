using ei8.Avatar.Installer.Domain.Model.DTO;
using ei8.Avatar.Installer.Domain.Model.External;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.IO.Process.Services.External;

public class NeuronPermitRepository : INeuronPermitRepository
{
    public async Task<IEnumerable<NeuronPermitDto>> GetNeuronPermitsAsync(string access)
    {
        var connectionString = $@"Data Source=file:{Path.Combine(access, "identity-access.db")}";
        var neuronPermits = new List<NeuronPermitDto>();
        var tableName = "NeuronPermit";

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        using var command = new SqliteCommand($"SELECT UserNeuronId, NeuronId, ExpirationDate FROM {tableName}", connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var permit = new NeuronPermitDto
            {
                UserNeuronId = reader["UserNeuronId"].ToString(),
                NeuronId = reader["NeuronId"].ToString(),
                ExpirationDate = reader["ExpirationDate"].ToString(),
            };

            neuronPermits.Add(permit);
        }

        return neuronPermits;
    }
}