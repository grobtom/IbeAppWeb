using IbeAppWeb;
using IbeAppWeb.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using Syncfusion.Blazor;

//Register Syncfusion license
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzg0OTQ2OEAzMjM5MmUzMDJlMzAzYjMyMzkzYkZOeTBCei9pZXhMVjJ4aG4xL0NaOFlmZjl1Q0hKQUI1U3VqU3hUdE01aWc9");
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddFluentUIComponents();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5000") });

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
});

builder.Services.AddScoped<ArbeitsscheinService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<UmsatzService>();
builder.Services.AddScoped<ProjectAnlageService>();
builder.Services.AddScoped<AnlagenService>();

builder.Services.AddSyncfusionBlazor();

await builder.Build().RunAsync();
