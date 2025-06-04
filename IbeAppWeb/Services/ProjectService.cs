using IbeAppWeb.DTOs;
using System.Net.Http.Json;

public class ProjectService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProjectService> _logger;

    public ProjectService(HttpClient httpClient, ILogger<ProjectService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<Project>?> GetProjectsAsync()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/IbeProject");
            request.Headers.Add("X-IbeProjectDB", "IbeProjects");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Project>>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching projects");
            return new List<Project>();
        }
    }

    public async Task<List<Project>?> GetActiveProjectsAsync()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/IbeProject/activ");
            request.Headers.Add("X-IbeProjectDB", "IbeProjects");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<Project>>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching active projects");
            return new List<Project>();
        }
    }

    public async Task<Project> UpdateProjectAsync(int projectId, bool isActive, bool isSchacht, int bauleiterId)
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

            var queryString = string.Join("&", queryParameters);

            var request = new HttpRequestMessage(HttpMethod.Put, $"api/IbeProject/update/{projectId}?{queryString}");
            request.Headers.Add("X-IbeProjectDB", "IbeProjects");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Project>() ?? new Project();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating project");
            return new Project();
        }
    }
}
