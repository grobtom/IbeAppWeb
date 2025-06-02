using IbeAppWeb;
using IbeAppWeb.Models;
using IbeAppWeb.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Charts;
using System.Globalization;

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzg0OTQ2OEAzMjM5MmUzMDJlMzAzYjMyMzkzYkZOeTBCei9pZXhMVjJ4aG4xL0NaOFlmZjl1Q0hKQUI1U3VqU3hUdE01aWc9");
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Services.AddFluentUIComponents();

builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

builder.Services.AddHttpClient("AuthorizedAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost/api");
})
.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthorizedAPI"));

builder.Services.AddMsalAuthentication<RemoteAuthenticationState, CustomUserAccount>(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("api://07d1f5eb-f995-4a62-8c28-084b579e01ed/ibeapp_api_all");
    options.ProviderOptions.DefaultAccessTokenScopes.Add("https://graph.microsoft.com/User.Read"); 
    options.UserOptions.RoleClaim = "appRole"; 
}).AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, CustomUserAccount, CustomAccountFactory>();


builder.Services.AddScoped<IbeToastService>();
builder.Services.AddScoped<ArbeitsscheinService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<UmsatzService>();
builder.Services.AddScoped<ProjectAnlageService>();
builder.Services.AddScoped<AnlagenService>();
builder.Services.AddScoped<MonteurService>();
builder.Services.AddScoped<BauleiterService>();

builder.Services.AddSyncfusionBlazor();
await builder.Build().RunAsync();
