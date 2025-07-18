# API Reference

Welcome to the IBE Application API Reference. This documentation provides comprehensive information about all the services, DTOs (Data Transfer Objects), and other types in the IBE Blazor WebAssembly application.

## Services

The application includes several service classes that handle communication with backend APIs:

### Core Services

- **[MonteurService](IbeAppWeb.Services.MonteurService.html)** - Complete CRUD operations for technician (Monteur) management
  - Retrieve, create, update, and delete Monteure
  - Assign Monteure to Anlagen (facilities)
  - Track Monteur history and project assignments

- **[AnlagenService](IbeAppWeb.Services.AnlagenService.html)** - Management of Anlage (facility) entities  
  - Full CRUD operations for facilities
  - Retrieve facilities with associated Monteure
  - Project-specific facility management
  - Soft delete and restoration capabilities

- **[BauleiterService](IbeAppWeb.Services.BauleiterService.html)** - Site manager (Bauleiter) operations
  - Manage site managers and their project assignments
  - Retrieve Bauleiter with associated projects

- **[ProjectService](IbeAppWeb.Services.ProjectService.html)** - Project management functionality
  - Handle project lifecycle operations
  - Customer and invoice associations
  - Active project filtering

- **[ArbeitsscheinService](IbeAppWeb.Services.ArbeitsscheinService.html)** - Work report management
  - Generate detailed work reports
  - Filter by facility, technician, and date ranges
  - Export capabilities for reports

## Data Transfer Objects (DTOs)

### Core Entity DTOs

- **[Project](IbeAppWeb.DTOs.Project.html)** - Main project entity with customer and Bauleiter associations
- **[BauleiterDto](IbeAppWeb.DTOs.BauleiterDto.html)** - Site manager information
- **[AnlageDto](IbeAppWeb.DTOs.AnlageDto.html)** - Facility entity details
- **[XCustomer](IbeAppWeb.DTOs.XCustomer.html)** - Customer entity with contact and address information
- **[Address](IbeAppWeb.DTOs.Address.html)** - Physical address representation

### Monteur DTOs

- **[MonteurResponse](IbeAppWeb.DTOs.Monteur.MonteurResponse.html)** - Technician entity with full details
- **[MonteurWithAnlageDto](IbeAppWeb.DTOs.MonteurWithAnlageDto.html)** - Technician with assigned facility
- **[ProjectMonteurDto](IbeAppWeb.DTOs.Monteur.ProjectMonteurDto.html)** - Project-technician relationship data

### Composite DTOs

- **[BauleiterWithProjectsDto](IbeAppWeb.DTOs.BauleiterWithProjectsDto.html)** - Site manager with associated projects
- **[ProjectCustomerInvoiceDto](IbeAppWeb.DTOs.ProjectCustomerInvoiceDto.html)** - Project with customer and invoice data
- **[AnlageWithMonteureDto](IbeAppWeb.DTOs.AnlageWithMonteureDto.html)** - Facility with assigned technicians
- **[ProjectWithAnlagenDto](IbeAppWeb.DTOs.ProjectWithAnlagenDto.html)** - Project with associated facilities

### Work Report DTOs

- **[ArbeitsscheinDto](IbeAppWeb.DTOs.ArbeitsscheinDto.html)** - Individual work sheet details
- **[ArbeitsberichtDbSummeDto](IbeAppWeb.DTOs.ArbeitsberichtDbSummeDto.html)** - Work report summary data
- **[UmsatzDto](IbeAppWeb.DTOs.UmsatzDto.html)** - Revenue calculation data

### Assignment DTOs

- **[AssignMonteurToAnlageDto](IbeAppWeb.DTOs.AssignMonteurToAnlageDto.html)** - Technician-facility assignment
- **[AssignAnlageToProjectDto](IbeAppWeb.DTOs.AssignAnlageToProjectDto.html)** - Facility-project assignment
- **[UpdateProjectAnlageDto](IbeAppWeb.DTOs.UpdateProjectAnlageDto.html)** - Project facility update operations

### History and Tracking

- **[MonteurAnlageHistoryDto](IbeAppWeb.DTOs.MonteurAnlageHistoryDto.html)** - Historical tracking of technician assignments
- **[XInvoice](IbeAppWeb.DTOs.XInvoice.html)** - Invoice tracking with project associations

## Validation

- **[ValidationErrorResponse](IbeAppWeb.Validation.ValidationErrorResponse.html)** - API validation error handling
- **[ValidationError](IbeAppWeb.Validation.ValidationError.html)** - Individual validation error details

## Key Features

### HTTP Client Services
All services use `HttpClient` for RESTful API communication with comprehensive error handling and response processing.

### Validation Support
Services include built-in validation error handling with detailed field-level error reporting.

### Async/Await Pattern
All service methods are asynchronous, following modern .NET patterns for non-blocking operations.

### Error Handling
Comprehensive exception handling with logging and user-friendly error messages.

### JSON Serialization
Full support for JSON serialization/deserialization with nullable reference types and proper attribute handling.

## Getting Started

To use these services in your Blazor components:

1. Inject the required service using `@inject`
2. Call service methods from component lifecycle methods or event handlers
3. Handle the returned data and any potential exceptions
4. Update component state and UI accordingly

Example: