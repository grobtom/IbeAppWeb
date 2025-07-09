using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace IbeAppWeb.Services;

/// <summary>
/// A custom message handler that integrates authorization functionality for HTTP requests.
/// </summary>
/// <remarks>This handler is designed to work with Blazor applications and ensures that HTTP requests include the
/// necessary authorization tokens. It automatically configures the handler to target specific authorized URLs and
/// scopes.  If an access token is not available, the handler redirects the user to reauthenticate.</remarks>
public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public CustomAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation)
        : base(provider, navigation)
    {
        ConfigureHandler(
            authorizedUrls: new[] { "https://localhost", "https://192.168.1.183", "https://ibeappweb.exrohr.dom" }
            , scopes: new[] { "api://07d1f5eb-f995-4a62-8c28-084b579e01ed/ibeapp_api_all" }
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
            ex.Redirect();
            throw;
        }
    }
}