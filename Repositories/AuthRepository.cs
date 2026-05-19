using System.Data;
using Microsoft.Data.SqlClient;
using WEBAPI_CRUD.Data;
using WEBAPI_CRUD.Models;

namespace WEBAPI_CRUD.Repositories;

public sealed class AuthRepository : IAuthRepository
{
    private readonly DBConnection _dbConnection;
    private readonly ILogger<AuthRepository> _logger;

    public AuthRepository(DBConnection dbConnection, ILogger<AuthRepository> logger)
    {
        _dbConnection = dbConnection;
        _logger = logger;
    }

    public AppUser? GetByEmail(string email)
    {
        const string sql = """
            SELECT Id, UserName, Email, PasswordHash, Role, IsActive
            FROM AppUsers
            WHERE Email = @Email;
            """;

        var table = _dbConnection.ExecuteDataTable(sql, [new SqlParameter("@Email", email)]);
        return table.Rows.Count == 0 ? null : MapUser(table.Rows[0]);
    }

    public AppUser Create(string userName, string email, string passwordHash, string role)
    {
        const string sql = """
            INSERT INTO AppUsers (UserName, Email, PasswordHash, Role, IsActive, CreatedAt)
            OUTPUT INSERTED.Id
            VALUES (@UserName, @Email, @PasswordHash, @Role, 1, SYSUTCDATETIME());
            """;

        var parameters = new[]
        {
            new SqlParameter("@UserName", userName),
            new SqlParameter("@Email", email),
            new SqlParameter("@PasswordHash", passwordHash),
            new SqlParameter("@Role", role)
        };

        var newId = Convert.ToInt32(_dbConnection.ExecuteScalar(sql, parameters));
        _logger.LogInformation("User created with id {UserId} and role {Role}", newId, role);

        return GetByEmail(email) ?? throw new InvalidOperationException("User was created but could not be loaded.");
    }

    private static AppUser MapUser(DataRow row) => new()
    {
        Id = row.Field<int>("Id"),
        UserName = row.Field<string>("UserName") ?? string.Empty,
        Email = row.Field<string>("Email") ?? string.Empty,
        PasswordHash = row.Field<string>("PasswordHash") ?? string.Empty,
        Role = row.Field<string>("Role") ?? "User",
        IsActive = row.Field<bool>("IsActive")
    };
}
