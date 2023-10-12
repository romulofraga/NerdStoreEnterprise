using Microsoft.AspNetCore.Http;

namespace NSE.WebApi.Core.Identidade
{
    public class CustomAuthorize
    {
        public static bool ValidarClaimsUsuario(HttpContext context, string claimName, string claimValue)
        {
            return context.User.Identity.IsAuthenticated && context.User.Claims.Any(claim => claim.Type == claimName && claim.Value.Contains(claimValue));
        }
    }
}
