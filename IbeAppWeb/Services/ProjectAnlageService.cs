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
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Assignment failed: {response.StatusCode} - {error}");
        }
    }

    public async Task<ProjectWithAnlagenDto> GetAnlagenForProject(int projectId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/ProjectAnlage/{projectId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"API returned {response.StatusCode} for project {projectId}");
            }

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                // Return empty project if no content
                return new ProjectWithAnlagenDto
                {
                    ProjectId = projectId,
                    ProjectName = "",
                    Anlagen = new List<AnlageDto>()
                };
            }

            var result = await response.Content.ReadFromJsonAsync<ProjectWithAnlagenDto>();

            if (result == null)
            {
                throw new HttpRequestException($"Failed to deserialize response for project {projectId}");
            }

            return result;
        }
        catch (System.Text.Json.JsonException jsonEx)
        {
            throw new HttpRequestException($"JSON parsing error for project {projectId}: {jsonEx.Message}");
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Failed to retrieve Anlagen for project {projectId}: {ex.Message}");
        }
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
        try
        {
            var response = await _httpClient.DeleteAsync($"api/ProjectAnlage/remove?projectId={projectId}&anlageId={anlageId}");
            
            var responseContent = await response.Content.ReadAsStringAsync();
            
            if (response.StatusCode == System.Net.HttpStatusCode.OK || 
                response.StatusCode == System.Net.HttpStatusCode.NoContent ||
                response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                return true;
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Removal failed: {response.StatusCode} - {error}");
            }
        }
        catch (HttpRequestException)
        {
            throw; 
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Unexpected error during removal: {ex.Message}", ex);
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