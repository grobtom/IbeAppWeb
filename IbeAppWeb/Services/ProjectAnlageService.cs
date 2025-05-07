using System.Net.Http.Json;
using IbeAppWeb.DTOs;

namespace IbeAppWeb.Services;

public class ProjectAnlageService
{
    private readonly HttpClient _httpClient;

    public ProjectAnlageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task AssignAnlageToProject(AssignAnlageToProjectDto dto)
    {
        await _httpClient.PostAsJsonAsync("api/ProjectAnlage/assign", dto);
    }

    public async Task<IEnumerable<ProjectAnlageDto>> GetAnlagenForProject(int projectId)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<ProjectAnlageDto>>($"api/ProjectAnlage/{projectId}");
    }

    public async Task<IEnumerable<ProjectWithAnlagenDto>> GetAllAnlagen()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<ProjectWithAnlagenDto>>($"api/ProjectAnlage/all");
    }

    public async Task UpdateAssignment(UpdateProjectAnlageDto dto)
    {
        await _httpClient.PutAsJsonAsync("api/ProjectAnlage/update", dto);
    }

    public async Task RemoveAssignment(int projectId, int anlageId)
    {
        await _httpClient.DeleteAsync($"api/ProjectAnlage/remove?projectId={projectId}&anlageId={anlageId}");
    }
}