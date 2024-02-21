using ei8.Avatar.Installer.Domain.Model.DTO;
using ei8.Avatar.Installer.Domain.Model.IdentityAccess;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.IO.Process.Services.IdentityAccess;

public class UserRepository : IUserRepository
{
    public async Task<IEnumerable<UserDto>> GetUsersAsync(string access)
    {
        var connectionString = $@"Data Source=file:{Path.Combine(access, "identity-access.db")}";
        var users = new List<UserDto>();
        var tableName = "User";

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        using var command = new SqliteCommand($"SELECT UserId, NeuronId, Active FROM {tableName}", connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var user = new UserDto
            {
                UserId = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                NeuronId = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                Active = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
            };

            users.Add(user);
        }

        return users; 
    }
}
