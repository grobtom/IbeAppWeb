# Getting Started

This guide will help you understand the IBE Application structure and get started with development.

## Prerequisites

- **.NET 9.0 SDK** - Latest .NET framework
- **Visual Studio 2022** or **Visual Studio Code**
- **Git** for version control
- **Node.js** (optional, for additional tooling)

## Project Structure

IbeAppWeb/ ├── DTOs/                  # Data Transfer Objects │   ├── Project.cs │   ├── Monteur/          # Monteur-specific DTOs │   └── ... ├── Services/             # HTTP Client Services │   ├── MonteurService.cs │   ├── AnlagenService.cs │   └── ... ├── Pages/                # Blazor Pages/Components │   ├── Monteure.razor │   ├── Bauleiter.razor │   └── ... ├── Shared/               # Shared Components ├── wwwroot/              # Static Assets ├── Program.cs            # Application Entry Point └── IbeAppWeb.csproj      # Project File

## Key Dependencies

### NuGet Packages

The application uses these key packages:

- **Microsoft.AspNetCore.Components.WebAssembly** - Blazor WebAssembly framework
- **Syncfusion.Blazor.*** - UI component suite
- **Microsoft.Authentication.WebAssembly.Msal** - Authentication
- **System.Net.Http.Json** - HTTP JSON operations

### Configuration

Review `IbeAppWeb.csproj` for all dependencies:

<PackageReference Include="Syncfusion.Blazor.Grid" Version="30.1.38" /> <PackageReference Include="Syncfusion.Blazor.Charts" Version="30.1.38" /> <PackageReference Include="Microsoft.FluentUI.AspNetCore.Components" Version="4.12.0" />

## Application Setup

### Program.cs Configuration

The main application configuration:

var builder = WebAssemblyHostBuilder.CreateDefault(args); builder.RootComponents.Add<App>("#app"); builder.RootComponents.Add<HeadOutlet>("head::after");
// HTTP Client Configuration builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
// Service Registration builder.Services.AddScoped<MonteurService>(); builder.Services.AddScoped<AnlagenService>(); builder.Services.AddScoped<ProjectService>(); builder.Services.AddScoped<BauleiterService>(); builder.Services.AddScoped<IbeToastService>();
// Syncfusion Configuration builder.Services.AddSyncfusionBlazor();
// Authentication (if enabled) builder.Services.AddMsalAuthentication(options => { builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication); });
await builder.Build().RunAsync();

## Development Workflow

### 1. Running the Application

The application will start at `https://localhost:5001` (or similar).

### 2. Creating New Components

Follow the established patterns when creating new components:

@page "/new-feature" @using IbeAppWeb.DTOs @using IbeAppWeb.Services @inject RequiredService Service @inject IbeToastService ToastService
<h3>New Feature</h3>
@if (isLoading) { <div>Loading...</div> } else { <!-- Component content --> }
@code { private bool isLoading = false;
private List<DataType> data = new();

protected override async Task OnInitializedAsync()
{
    await LoadData();
}

private async Task LoadData()
{
    isLoading = true;
    try
    {
        data = await Service.GetData();
    }
    catch (Exception ex)
    {
        await ToastService.ShowToast($"Error: {ex.Message}", false);
    }
    finally
    {
        isLoading = false;
    }
}
}

### 3. Adding New Services

Create services following the established pattern:

public class NewService { private readonly HttpClient _httpClient;
public NewService(HttpClient httpClient)
{
    _httpClient = httpClient;
}

public async Task<IEnumerable<DataType>> GetData()
{
    try
    {
        var response = await _httpClient.GetAsync("api/endpoint");
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<DataType>>() 
                   ?? Enumerable.Empty<DataType>();
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"API Error: {response.StatusCode}, {errorContent}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in GetData: {ex.Message}");
        return Enumerable.Empty<DataType>();
    }
}
}

## Common Patterns

### Data Loading Pattern

Standard pattern for loading data in components:

private async Task LoadData() { isLoading = true; try { data = await Service.GetData(); await ToastService.ShowToast("Data loaded successfully", true); } catch (Exception ex) { await ToastService.ShowToast($"Error loading data: {ex.Message}", false); } finally { isLoading = false; StateHasChanged(); } }

### CRUD Operations Pattern

Consistent pattern for Create, Read, Update, Delete operations:

private async Task Create(EntityType entity) { try { var result = await Service.Create(entity); if (result != null) { await LoadData(); // Refresh the list await ToastService.ShowToast("Created successfully", true); } } catch (Exception ex) { await ToastService.ShowToast($"Error creating: {ex.Message}", false); } }

### Validation Pattern

Use DataAnnotations for validation:

public class EntityModel { [Required(ErrorMessage = "Name is required")] [MinLength(3, ErrorMessage = "Name must be at least 3 characters")] public string Name { get; set; } = string.Empty;
[Required(ErrorMessage = "Email is required")]
[EmailAddress(ErrorMessage = "Invalid email format")]
public string Email { get; set; } = string.Empty;
}

## Styling and Themes

### Syncfusion Themes

The application uses Syncfusion themes. Current theme configuration:

<!-- In index.html --> <link href="_content/Syncfusion.Blazor.Themes/bootstrap5.css" rel="stylesheet" />

### Custom Styling

Add custom CSS for application-specific styling:

.e-grid .e-headercell { background-color: #a2d6f4; color: rgb(3, 2, 2); }
.custom-button { background-color: #007bff; border-color: #007bff; }

## Debugging and Troubleshooting

### Common Issues

1. **Service Not Found** - Ensure services are registered in `Program.cs`
2. **CORS Issues** - Check API configuration for cross-origin requests
3. **Authentication Errors** - Verify MSAL configuration
4. **Component Not Updating** - Call `StateHasChanged()` after data changes

### Development Tools

- **Browser DevTools** - Use F12 for debugging JavaScript and network requests
- **Visual Studio Debugger** - Set breakpoints in C# code
- **Blazor DevTools** - Browser extension for Blazor-specific debugging

### Logging

Add logging for debugging:

Console.WriteLine($"Debug info: {variable}"); // Or use ILogger if configured

## Deployment

### Build for Production

dotnet publish -c Release -o publish

### Static File Hosting

The built application can be hosted on any static file server:
- Azure Static Web Apps
- GitHub Pages
- IIS
- Apache/Nginx

## Next Steps

1. **Explore Components** - Review existing Blazor components
2. **Understand Services** - Study the service layer architecture
3. **Read API Documentation** - Check the generated API documentation
4. **Start Development** - Create your first component or service
5. **Run Tests** - Set up and run the test suite

## Getting Help

- **Documentation** - This documentation site
- **Code Comments** - Extensive XML documentation in code
- **Team Knowledge** - Consult with team members
- **Official Docs** - Blazor and Syncfusion documentation

Happy coding! 🚀

