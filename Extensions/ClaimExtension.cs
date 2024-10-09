using System.Security.Claims;

namespace FinShark.Extensions
{
    public static class ClaimExtension
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            var givenNameClaim = user.Claims.FirstOrDefault(x =>
                    x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
            if (givenNameClaim != null) return givenNameClaim.Value;
            return null;

        }
    }
}
