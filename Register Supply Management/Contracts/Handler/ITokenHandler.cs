using System.Security.Claims;

namespace Register_Supply_Management.Contracts.Handler
{
    public interface ITokenHandler
    {
        public string GenerateToken(IEnumerable<Claim> claims);
    }
}
