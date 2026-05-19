using WEBAPI_CRUD.DTOs;
using WEBAPI_CRUD.Models;

namespace WEBAPI_CRUD.Services;

public interface IJwtTokenService
{
    AuthResponse GenerateToken(AppUser user);
}
