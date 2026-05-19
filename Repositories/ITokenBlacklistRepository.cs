namespace WEBAPI_CRUD.Repositories;

public interface ITokenBlacklistRepository
{
    void Revoke(string tokenId, DateTime expiresAt);
    bool IsRevoked(string tokenId);
}
