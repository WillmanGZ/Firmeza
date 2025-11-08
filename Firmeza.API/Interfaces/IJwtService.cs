using Microsoft.AspNetCore.Identity;

namespace Firmeza.API.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(IdentityUser user, IList<string> roles);
    }
}
