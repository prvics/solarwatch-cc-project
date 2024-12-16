using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Services;

public interface ITokenService
{
    public string CreateToken(IdentityUser user, string? role);
}