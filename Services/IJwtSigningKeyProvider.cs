namespace WEBAPI_CRUD.Services;

public interface IJwtSigningKeyProvider
{
    string GetSigningKey();
}
