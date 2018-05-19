using System.Linq;
using System.Security.Claims;

namespace WebStore.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetId(this ClaimsPrincipal user)
        {
            return int.Parse(user.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}