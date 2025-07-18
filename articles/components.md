# Blazor Components

The IBE Application uses Blazor WebAssembly with Syncfusion components to create rich, interactive user interfaces. This section covers the main component patterns and implementations.

## Component Architecture

### Base Component Structure

All major components follow a consistent structure:

@page "/route" @using Directives @inject Services
<ComponentMarkup> <!-- UI Elements --> </ComponentMarkup>
@code { // Component logic }

### Key Features

- **Reactive Data Binding** - Two-way data binding with `@bind`
- **Event Handling** - Async event handlers for user interactions
- **State Management** - Local component state with `StateHasChanged()`
- **Lifecycle Methods** - `OnInitializedAsync()` for component initialization
- **Dependency Injection** - Service injection for data access

## Core Components

### Monteure.razor - Technician Management

The main component for managing technicians with comprehensive CRUD operations.

**Features:**
- **Data Grid** - Syncfusion SfGrid with paging, sorting, and filtering
- **Modal Dialogs** - Add/Edit technician information
- **Detail Views** - Expandable rows showing project assignments
- **Validation** - Real-time input validation
- **Toast Notifications** - User feedback for operations

**Key Implementation Details:**

<SfGrid @ref="Grid" DataSource="@Data" AllowPaging="true" Toolbar="@(new List<string>() { "Edit", "Delete", "Cancel" })">
<GridEditSettings AllowDeleting="true" AllowEditing="true" Mode="EditMode.Dialog" />
<GridEvents TValue="MonteurResponse" OnActionBegin="OnActionBegin" />

<GridTemplates>
    <DetailTemplate Context="context">
        @{
            var monteur = context as MonteurResponse;
            var filteredProjects = Projects.Where(p => p.MonteurId == monteur.MonteurId).ToList();
        }
        <SfGrid DataSource="@filteredProjects">
            <!-- Project details grid -->
        </SfGrid>
    </DetailTemplate>
</GridTemplates>

<!-- Column definitions -->
<GridColumns>
    <GridColumn Field="@nameof(MonteurResponse.MonteurId)" HeaderText="ID" IsPrimaryKey="true" />
    <GridColumn Field="@nameof(MonteurResponse.Vorname)" HeaderText="First Name" 
               ValidationRules="@(new ValidationRules { Required = true, MinLength = 3 })" />
    <!-- Additional columns -->
</GridColumns>
</SfGrid>

### Master-Detail Pattern

The application extensively uses master-detail relationships:

**Implementation:**
- **Master Grid** - Shows primary entities (e.g., Technicians)
- **Detail Template** - Expandable content showing related data
- **Data Filtering** - Details filtered by master record ID
- **Lazy Loading** - Details loaded only when expanded

### Form Components

#### Data Entry Forms

Standard pattern for data entry with validation:

<EditForm Model="@formModel" OnValidSubmit="HandleSubmit"> <DataAnnotationsValidator /> <ValidationSummary />
<div class="form-group">
    <label>First Name:</label>
    <InputText @bind-Value="formModel.Vorname" class="form-control" />
    <ValidationMessage For="@(() => formModel.Vorname)" />
</div>

<button type="submit" class="btn btn-primary">Save</button>
</EditForm>

#### Custom Validation

Components support both client-side and server-side validation:

[Required]
[EmailAddress]
public string? Email { get; set; }

public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
{
    // Custom validation logic
    if (SomeCondition)
    {
        yield return new ValidationResult("Custom error message", new[] { nameof(PropertyName) });
    }
}
}

## Grid Components

### Syncfusion SfGrid Features

The application leverages advanced SfGrid capabilities:

**Data Operations:**
- **Paging** - Large dataset pagination
- **Sorting** - Multi-column sorting support
- **Filtering** - Built-in filter menu
- **Grouping** - Hierarchical data grouping
- **Selection** - Single and multi-row selection

**Editing Features:**
- **Inline Editing** - Direct cell editing
- **Dialog Editing** - Modal dialog forms
- **Batch Editing** - Multiple row changes
- **Validation** - Integrated validation rules

**Export Capabilities:**
- **PDF Export** - Formatted PDF generation
- **Excel Export** - Spreadsheet export
- **Custom Templates** - Branded export layouts

### Grid Event Handling

private async Task OnActionBegin(ActionEventArgs<EntityType> args) { if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save) { args.Cancel = true; // Prevent default behavior
    if (args.Action == "Add")
    {
        await CustomAdd(args.Data);
    }
    else if (args.Action == "Edit")
    {
        await CustomUpdate(args.Data);
    }
    
    await Grid.CloseEditAsync();
}
else if (args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
{
    await CustomDelete(args.Data);
}
}

## Navigation and Routing

### Page Routes

Components use attribute-based routing:

@page "/monteure" @page "/monteure/{id:int}"

### Navigation Services

Programmatic navigation using `NavigationManager`:

@inject NavigationManager Navigation
private void NavigateToDetails(int id) { Navigation.NavigateTo($"/monteure/{id}"); }

## State Management

### Component State

Local state management patterns:

@code { private bool isLoading = false; private List<DataType> items = new(); private string? errorMessage;
protected override async Task OnInitializedAsync()
{
    await LoadData();
}

private async Task LoadData()
{
    isLoading = true;
    StateHasChanged(); // Trigger UI update
    
    try
    {
        items = await Service.GetData();
    }
    catch (Exception ex)
    {
        errorMessage = ex.Message;
    }
    finally
    {
        isLoading = false;
        StateHasChanged();
    }
}
}

### Global State

For application-wide state, consider:
- **Singleton Services** - Shared data services
- **Event Callbacks** - Parent-child communication
- **State Containers** - Custom state management

## Performance Optimization

### Component Optimization

**Best Practices:**
- Use `@key` directive for list items
- Implement `ShouldRender()` for conditional rendering
- Minimize unnecessary re-renders
- Use `EventCallback` for parent-child communication

### Data Loading

**Efficient Data Patterns:**
- Lazy loading for large datasets
- Virtual scrolling for grids
- Progressive data loading
- Caching frequently accessed data

private async Task<IEnumerable<DataType>> LoadDataLazy(int page, int pageSize) { return await Service.GetPagedData(page, pageSize); }


## Testing Components

### Component Testing

Use bUnit for component testing:

[Test] public void Component_RendersCorrectly() { // Arrange using var ctx = new TestContext(); ctx.Services.AddSingleton<IService>(mockService);
// Act
var component = ctx.RenderComponent<MyComponent>();

// Assert
Assert.That(component.Find("h1").TextContent, Is.EqualTo("Expected Title"));
}

### Integration Testing

Test component interactions with services and navigation.

## Accessibility

### ARIA Support

Ensure components are accessible:
- Use semantic HTML elements
- Provide ARIA labels and descriptions
- Support keyboard navigation
- Maintain proper focus management

### Syncfusion Accessibility

Syncfusion components include built-in accessibility features:
- Screen reader support
- Keyboard navigation
- High contrast themes
- WCAG compliance

