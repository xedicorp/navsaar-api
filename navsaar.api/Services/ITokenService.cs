
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
namespace navsaar.api.Services
{
 
    public interface ITokenService
    {


        public string GenerateToken(string username, string role);
        public int GetUserIdFromToken();
    }

}
