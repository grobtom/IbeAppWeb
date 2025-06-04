using IbeAppWeb.DTOs;
using System.Net.Http.Json;

namespace IbeAppWeb.Services;

public class ProjectAnlageService
{
    private readonly HttpClient _httpClient;

    public ProjectAnlageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> AssignAnlageToProject(AssignAnlageToProjectDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/ProjectAnlage/assign", dto);
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            // Optionally log or handle the error here
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Assignment failed: {response.StatusCode} - {error}");
        }
    }

    public async Task<ProjectWithAnlagenDto> GetAnlagenForProject(int projectId)
    {
        var response = await _httpClient.GetFromJsonAsync<ProjectWithAnlagenDto>($"api/ProjectAnlage/{projectId}");
        if (response == null)
        {
            throw new HttpRequestException($"Failed to retrieve Anlagen for project {projectId}.");
        }
        return response;
    }

    public async Task<IEnumerable<ProjectWithAnlagenDto>> GetAllProjectAnlagen()
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<ProjectWithAnlagenDto>>($"api/ProjectAnlage/all");
        if (response == null)
        {
            throw new HttpRequestException("Failed to retrieve projects with Anlagen.");
        }
        return response.Where(p => p.Anlagen != null).ToList();
    }

    public async Task<bool> UpdateAssignment(UpdateProjectAnlageDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync("api/ProjectAnlage/update", dto);
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            // Optionally log or handle the error here
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Update failed: {response.StatusCode} - {error}");
        }
    }

    public async Task<bool> RemoveAssignment(int projectId, int anlageId)
    {
        var response = await _httpClient.DeleteAsync($"api/ProjectAnlage/remove?projectId={projectId}&anlageId={anlageId}");
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            // Optionally log or handle the error here
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Removal failed: {response.StatusCode} - {error}");
        }
    }

    public async Task<List<AnlageDto>> GetAllAnlagen()
    {
        var response = await _httpClient.GetFromJsonAsync<List<AnlageDto>>($"api/Anlage");
        if (response == null)
        {
            throw new HttpRequestException("Failed to retrieve Anlagen.");
        }
        return response.Where(a => !a.IsDeleted).ToList();
    }
}