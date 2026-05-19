using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEBAPI_CRUD.DTOs;
using WEBAPI_CRUD.Repositories;
using WEBAPI_CRUD.Services;

namespace WEBAPI_CRUD.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepository;
    private readonly ITokenBlacklistRepository _tokenBlacklistRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthRepository authRepository,
        ITokenBlacklistRepository tokenBlacklistRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        ILogger<AuthController> logger)
    {
        _authRepository = authRepository;
        _tokenBlacklistRepository = tokenBlacklistRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public ActionResult<AuthResponse> Login(LoginRequest request)
    {
        var user = _authRepository.GetByEmail(request.Email);
        if (user is null || !user.IsActive || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            _logger.LogWarning("Invalid login attempt for {Email}", request.Email);
            return Unauthorized(new { message = "Invalid email or password." });
        }

        return Ok(_jwtTokenService.GenerateToken(user));
    }

    [HttpPost("register")]
    [Authorize(Policy = "AdminOnly")]
    public ActionResult<AuthResponse> Register(RegisterRequest request)
    {
        if (_authRepository.GetByEmail(request.Email) is not null)
        {
            return Conflict(new { message = "A user with this email already exists." });
        }

        var user = _authRepository.Create(
            request.UserName,
            request.Email,
            _passwordHasher.Hash(request.Password),
            request.Role);

        return CreatedAtAction(nameof(Login), new { email = user.Email }, _jwtTokenService.GenerateToken(user));
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        var tokenId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        if (string.IsNullOrWhiteSpace(tokenId))
        {
            return BadRequest(new { message = "Token id was not found." });
        }

        var expiresAt = GetCurrentTokenExpiry();
        _tokenBlacklistRepository.Revoke(tokenId, expiresAt);

        return Ok(new { message = "Logged out successfully." });
    }

    private DateTime GetCurrentTokenExpiry()
    {
        var authorizationHeader = Request.Headers.Authorization.ToString();
        var token = authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
            ? authorizationHeader["Bearer ".Length..].Trim()
            : string.Empty;

        if (string.IsNullOrWhiteSpace(token))
        {
            return DateTime.UtcNow;
        }

        return new JwtSecurityTokenHandler().ReadJwtToken(token).ValidTo;
    }
}
