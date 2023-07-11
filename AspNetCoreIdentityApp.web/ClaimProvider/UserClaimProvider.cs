using AspNetCoreIdentityApp.web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.web.ClaimProvider
{
    public class UserClaimProvider : IClaimsTransformation
    {
        private readonly UserManager<AppUser> _userManager;

        public UserClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identityUser = (principal.Identity as ClaimsIdentity)!;
            var currentUSer = await _userManager.FindByNameAsync(identityUser.Name!);

            if (currentUSer == null)
            {
                return principal;
            }

            if (String.IsNullOrEmpty(currentUSer.City))
            {
                return principal;
            }


            if (principal.HasClaim(x => x.Type != "city"))
            {
                Claim cityClaim = new Claim("city", currentUSer.City);
                identityUser.AddClaim(cityClaim);
            }

            return principal;
        }
    }
}
