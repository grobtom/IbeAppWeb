using IbeAppWeb;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;

//Register Syncfusion license
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzc3NzIyMUAzMjM5MmUzMDJlMzAzYjMyMzkzYkZ6emxzRUg4N3hraFZnUkROdnc1d24xL1VkbmF0Z0wxY1lLNVRKc24ySGs9");
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5000") });

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
});

builder.Services.AddScoped<ArbeitsscheinService>();
builder.Services.AddScoped<ProjectService>();

builder.Services.AddSyncfusionBlazor();

await builder.Build().RunAsync();
