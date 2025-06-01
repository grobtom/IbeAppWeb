using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace IbeAppWeb.Services;
public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public CustomAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation)
        : base(provider, navigation)
    {
        ConfigureHandler(
            authorizedUrls: new[] { "https://localhost" }
            , scopes: new[] {"api://07d1f5eb-f995-4a62-8c28-084b579e01ed/ibeapp_api_all"} // Uncomment and set if needed
        );
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect(); // This will redirect the user to login
            throw; // Optionally rethrow or handle as needed
        }
    }
}