﻿using ei8.Avatar.Installer.Domain.Model.DTO;
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
    public async Task<IEnumerable<User>> GetUsersAsync(string access)
    {
        var connectionString = $@"Data Source=file:{Path.Combine(access, "identity-access.db")}";
        var users = new List<User>();
        var tableName = "User";

        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        using var command = new SqliteCommand($"SELECT UserId, NeuronId, Active FROM {tableName}", connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var user = new User(
                reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                reader.IsDBNull(2) ? null : reader.GetInt32(2));

            users.Add(user);
        }

        return users;
    }

    public async Task UpdateUserAsync(string access, User user)
    {
        var connectionString = $@"Data Source=file:{Path.Combine(access, "identity-access.db")}";
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
