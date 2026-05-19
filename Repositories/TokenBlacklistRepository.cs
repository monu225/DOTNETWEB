using Microsoft.Data.SqlClient;
using WEBAPI_CRUD.Data;

namespace WEBAPI_CRUD.Repositories;

public sealed class TokenBlacklistRepository : ITokenBlacklistRepository
{
    private readonly DBConnection _dbConnection;
    private readonly ILogger<TokenBlacklistRepository> _logger;

    public TokenBlacklistRepository(DBConnection dbConnection, ILogger<TokenBlacklistRepository> logger)
    {
        _dbConnection = dbConnection;
        _logger = logger;
    }

    public void Revoke(string tokenId, DateTime expiresAt)
    {
        const string sql = """
            IF NOT EXISTS (SELECT 1 FROM RevokedTokens WHERE TokenId = @TokenId)
            BEGIN
                INSERT INTO RevokedTokens (TokenId, ExpiresAt, RevokedAt)
                VALUES (@TokenId, @ExpiresAt, SYSUTCDATETIME());
            END

            DELETE FROM RevokedTokens
            WHERE ExpiresAt <= SYSUTCDATETIME();
            """;

        var parameters = new[]
        {
            new SqlParameter("@TokenId", tokenId),
            new SqlParameter("@ExpiresAt", expiresAt)
        };

        _dbConnection.ExecuteNonQuery(sql, parameters);
        _logger.LogInformation("JWT token {TokenId} was revoked until {ExpiresAt}.", tokenId, expiresAt);
    }

    public bool IsRevoked(string tokenId)
    {
        const string sql = """
            SELECT COUNT(1)
            FROM RevokedTokens
            WHERE TokenId = @TokenId
              AND ExpiresAt > SYSUTCDATETIME();
            """;

        var count = Convert.ToInt32(_dbConnection.ExecuteScalar(sql, [new SqlParameter("@TokenId", tokenId)]));
        return count > 0;
    }
}
