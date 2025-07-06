using IbeAppWeb;
using IbeAppWeb.Models;
using IbeAppWeb.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor;
using System.Globalization;

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzkyMTMwOEAzMzMwMmUzMDJlMzAzYjMzMzAzYmtYZk91V2J1emthTWxveUF3OS9mNzZjRDlQWXJWR2lLOWZJNmFid0VCWms9");
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Logging.SetMinimumLevel(LogLevel.Information);

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
    options.UserOptions.RoleClaim = "appRole";
}).AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, CustomUserAccount, CustomAccountFactory>();

builder.Services.AddSingleton(typeof(ISyncfusionStringLocalizer), typeof(SyncfusionLocalizer));
builder.Services.AddScoped<IbeToastService>();
builder.Services.AddScoped<ArbeitsscheinService>();
builder.Services.AddScoped<ProjectService>(sp =>
{
    var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = clientFactory.CreateClient("AuthorizedAPI");
    var logger = sp.GetRequiredService<ILogger<ProjectService>>();
    return new ProjectService(httpClient, logger);
});
builder.Services.AddScoped<UmsatzService>();
builder.Services.AddScoped<ProjectAnlageService>();
builder.Services.AddScoped<AnlagenService>();
builder.Services.AddScoped<MonteurService>();
builder.Services.AddScoped<BauleiterService>();
builder.Services.AddScoped<RechnungService>();

builder.Services.AddSyncfusionBlazor();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources"); 

var host = builder.Build();

const string defaultCulture = "en-US";

var js = host.Services.GetRequiredService<IJSRuntime>();
var result = await js.InvokeAsync<string>("blazorCulture.get");
var culture = CultureInfo.GetCultureInfo(result ?? defaultCulture);

if (result == null)
{
    await js.InvokeVoidAsync("blazorCulture.set", defaultCulture);
}

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();