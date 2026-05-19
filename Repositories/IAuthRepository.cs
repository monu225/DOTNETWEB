using WEBAPI_CRUD.Models;

namespace WEBAPI_CRUD.Repositories;

public interface IAuthRepository
{
    AppUser? GetByEmail(string email);
    AppUser Create(string userName, string email, string passwordHash, string role);
}
