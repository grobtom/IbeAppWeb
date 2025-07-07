using IbeAppWeb.DTOs;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

public class ProjectService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProjectService> _logger;

    public ProjectService(HttpClient httpClient, ILogger<ProjectService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<ProjectCustomerInvoiceDto>?> GetProjectsAsync()
    {
        try
        {
            _logger.LogInformation("Attempting to fetch projects from API");
            _logger.LogInformation($"API Base URL: {_httpClient.BaseAddress}");

            var request = new HttpRequestMessage(HttpMethod.Get, "api/IbeProject");
            request.Headers.Add("X-IbeProjectDB", "IbeProjects");

            _logger.LogInformation($"Making request to: {request.RequestUri}");

            var response = await _httpClient.SendAsync(request);

            _logger.LogInformation($"Response status: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"API returned error status {response.StatusCode}: {errorContent}");
                return new List<ProjectCustomerInvoiceDto>();
            }

            var contentString = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Raw response content (first 500 chars): {contentString.Substring(0, Math.Min(500, contentString.Length))}");

            // Check if response is valid JSON
            if (string.IsNullOrEmpty(contentString) || contentString.Trim().Length == 0)
            {
                _logger.LogWarning("API returned empty response");
                return new List<ProjectCustomerInvoiceDto>();
            }

            // Try to parse JSON manually to get better error information
            try
            {
                var result = System.Text.Json.JsonSerializer.Deserialize<List<ProjectCustomerInvoiceDto>>(contentString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                _logger.LogInformation($"Successfully parsed {result?.Count ?? 0} projects");
                return result ?? new List<ProjectCustomerInvoiceDto>();
            }
            catch (System.Text.Json.JsonException jsonEx)
            {
                _logger.LogError(jsonEx, $"JSON parsing error. Response content: {contentString}");
                return new List<ProjectCustomerInvoiceDto>();
            }
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "HTTP request error when fetching projects");
            return new List<ProjectCustomerInvoiceDto>();
        }
        catch (TaskCanceledException tcEx)
        {
            _logger.LogError(tcEx, "Request timeout when fetching projects");
            return new List<ProjectCustomerInvoiceDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching projects");
            return new List<ProjectCustomerInvoiceDto>();
        }
    }

    public async Task<List<ProjectCustomerInvoiceDto>?> GetActiveProjectsAsync()
    {
        try
        {
            _logger.LogInformation("Attempting to fetch active projects from API");

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/IbeProject/activ");
            request.Headers.Add("X-IbeProjectDB", "IbeProjects");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"API returned error status {response.StatusCode}: {errorContent}");
                return new List<ProjectCustomerInvoiceDto>();
            }

            var contentString = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Active projects response (first 200 chars): {contentString.Substring(0, Math.Min(200, contentString.Length))}");

            return System.Text.Json.JsonSerializer.Deserialize<List<ProjectCustomerInvoiceDto>>(contentString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new List<ProjectCustomerInvoiceDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching active projects");
            return new List<ProjectCustomerInvoiceDto>();
        }
    }

    public async Task<List<Project>?> GetActiveProjectsSimpleAsync()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/IbeProject/activesimple");
            request.Headers.Add("X-IbeProjectDB", "IbeProjects");
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"API returned error status {response.StatusCode}: {errorContent}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<List<Project>>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching active simple projects");
            return null;
        }
    }

    public async Task<List<Project>?> GetProjectsSimpleAsync()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/IbeProject/simple");
            request.Headers.Add("X-IbeProjectDB", "IbeProjects");
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"API returned error status {response.StatusCode}: {errorContent}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<List<Project>>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching simple projects");
            return null;
        }
    }

    public async Task<Project> UpdateProjectAsync(int projectId, bool isActive, bool isSchacht, int bauleiterId, int customerId, string fileUrl = "")
    {
        try
        {
            var queryParameters = new List<string>();
            queryParameters.Add($"dbActive={isActive}");
            queryParameters.Add($"schacht={isSchacht}");
            if (bauleiterId > 0)
            {
                queryParameters.Add($"bauleiterId={bauleiterId}");
            }
            if (customerId > 0)
            {
                queryParameters.Add($"customerId={customerId}");
            }
            if (!string.IsNullOrEmpty(fileUrl))
            {
                queryParameters.Add($"fileUrl={Uri.EscapeDataString(fileUrl)}");
            }
            var queryString = string.Join("&", queryParameters);

            var request = new HttpRequestMessage(HttpMethod.Put, $"api/IbeProject/update/{projectId}?{queryString}");
            request.Headers.Add("X-IbeProjectDB", "IbeProjects");

            _logger.LogInformation($"Updating project {projectId} with URL: {request.RequestUri}");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Failed to update project {projectId}. Status: {response.StatusCode}, Content: {errorContent}");
                return new Project();
            }

            return await response.Content.ReadFromJsonAsync<Project>() ?? new Project();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating project {projectId}");
            return new Project();
        }
    }
}