using ei8.Avatar.Installer.Domain.Model;
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
    private readonly IAvatarContextService avatarContextService;

    public UserRepository(IAvatarContextService avatarContextService)
    {
        this.avatarContextService = avatarContextService;
    }

    public async Task<IEnumerable<User>> GetAllAsync(string access)
    {
        var id = avatarContextService.Avatar!.Id;
        var connectionString = $@"Data Source=file:{Path.Combine(id, "identity-access.db")}";
        var users = new List<User>();
        var tableName = "User";

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        using var command = new SqliteCommand($"SELECT UserId, NeuronId, Active FROM {tableName}", connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var user = new User
            {
                UserId = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                NeuronId = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                Active = reader.IsDBNull(2) ? null : reader.GetInt32(2)
            };

            users.Add(user);
        }

        return users;
    }

    public async Task UpdateAsync(string access, User user)
    {
        var id = avatarContextService.Avatar!.Id;
        var connectionString = $@"Data Source=file:{Path.Combine(id, "identity-access.db")}";
        var tableName = "User";

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var query = $"UPDATE {tableName} SET NeuronId = @NeuronId, Active = @Active WHERE UserId = @UserId";
        using var command = new SqliteCommand(query, connection);

        command.Parameters.AddWithValue("@NeuronId", user.NeuronId);
        command.Parameters.AddWithValue("@Active", user.Active is null ? DBNull.Value : user.Active);

        command.Parameters.AddWithValue("@UserId", user.UserId);

        await command.ExecuteNonQueryAsync();
    }
}
