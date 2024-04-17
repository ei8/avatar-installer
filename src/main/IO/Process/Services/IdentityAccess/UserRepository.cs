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

public class UserRepository : IUserRepository
{
    private readonly IAvatarContextService avatarContextService;

    public UserRepository(IAvatarContextService avatarContextService)
    {
        AssertionConcern.AssertArgumentNotNull(avatarContextService, nameof(avatarContextService));

        this.avatarContextService = avatarContextService;
    }

    public async Task<User> GetByIdAsync(string userId)
    {
        AssertionConcern.AssertArgumentNotNull(userId, nameof(userId));

        var id = avatarContextService.Avatar.Id;
        var connectionString = $@"Data Source=file:{Path.Combine(id, Constants.Databases.IdentityAccessDb)}";

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var query = $"SELECT UserId, NeuronId, Active FROM {Constants.TableNames.User} WHERE UserId = @UserId";
        using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@UserId", userId);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var user = new User
            {
                UserId = reader.GetString(0),
                NeuronId = reader.GetString(1),
                Active = reader.IsDBNull(2) ? null : reader.GetInt32(2)
            };
            return user;
        }

        return null;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var id = avatarContextService.Avatar.Id;
        var connectionString = $@"Data Source=file:{Path.Combine(id, Constants.Databases.IdentityAccessDb)}";
        var users = new List<User>();

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        using var command = new SqliteCommand($"SELECT UserId, NeuronId, Active FROM {Constants.TableNames.User}", connection);
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

    public async Task RemoveAsync(User user)
    {
        AssertionConcern.AssertArgumentNotNull(user, nameof(user));

        var id = avatarContextService.Avatar.Id;
        var connectionString = $@"Data Source=file:{Path.Combine(id, Constants.Databases.IdentityAccessDb)}";

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var query = $"DELETE FROM {Constants.TableNames.User} WHERE UserId = @UserId";
        using var command = new SqliteCommand(query, connection);

        command.Parameters.AddWithValue("@UserId", user.UserId);

        await command.ExecuteNonQueryAsync();
    }

    public async Task SaveAsync(User user)
    {
        AssertionConcern.AssertArgumentNotNull(user, nameof(user));

        var id = avatarContextService.Avatar.Id;
        var connectionString = $@"Data Source=file:{Path.Combine(id, Constants.Databases.IdentityAccessDb)}";

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var query = $@"
            INSERT OR REPLACE INTO {Constants.TableNames.User} (UserId, NeuronId, Active)
            VALUES (@UserId, @NeuronId, @Active)";
        using var command = new SqliteCommand(query, connection);

        command.Parameters.AddWithValue("@UserId", user.UserId);
        command.Parameters.AddWithValue("@NeuronId", user.NeuronId);
        command.Parameters.AddWithValue("@Active", user.Active is null ? DBNull.Value : (object)user.Active);

        await command.ExecuteNonQueryAsync();
    }
}
