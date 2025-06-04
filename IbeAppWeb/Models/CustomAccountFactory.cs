using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System.Security.Claims;

namespace IbeAppWeb.Models;

public class CustomAccountFactory : AccountClaimsPrincipalFactory<CustomUserAccount>
{
    public CustomAccountFactory(IAccessTokenProviderAccessor accessor)
        : base(accessor)
    {
    }


    public override async ValueTask<ClaimsPrincipal> CreateUserAsync(CustomUserAccount account, RemoteAuthenticationUserOptions options)
    {
        var initialUser = await base.CreateUserAsync(account, options);

        if (initialUser?.Identity?.IsAuthenticated ?? false)
        {
            var userIdentity = (ClaimsIdentity)initialUser.Identity;

            userIdentity.AddClaim(new Claim("appRole", string.Join(",", account.Roles ?? new List<string>())));
        }
        return initialUser;
    }
}


