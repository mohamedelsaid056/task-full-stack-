
using Core.Models.Auth;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user);
    }
}