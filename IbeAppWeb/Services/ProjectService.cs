using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using IbeAppWeb.DTOs;
using Microsoft.Extensions.Logging;

public class ProjectService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProjectService> _logger;

    public ProjectService(HttpClient httpClient, ILogger<ProjectService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
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
}
