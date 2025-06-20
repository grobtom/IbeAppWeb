using IbeAppWeb.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;

namespace IbeAppWeb.Services;

public class BauleiterService
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;

    public BauleiterService(HttpClient httpClient, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
    }
    public async Task<IEnumerable<BauleiterDto>> GetAllBauleiter()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Bauleiter");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<BauleiterDto>>() ?? Enumerable.Empty<BauleiterDto>();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to fetch Bauleiters. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllBauleiters: {ex.Message}");
            return Enumerable.Empty<BauleiterDto>();
        }
    }

    public async Task<IEnumerable<BauleiterWithProjectsDto>> GetBauleiterWithProjects()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Bauleiter/with-projects");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<BauleiterWithProjectsDto>>() ?? Enumerable.Empty<BauleiterWithProjectsDto>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new AccessTokenNotAvailableException(
                    _navigationManager,
                    null,
                    new[] { "api.read" }
                );
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to fetch Bauleiters with projects. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            return Enumerable.Empty<BauleiterWithProjectsDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetBauleiterWithProjects: {ex.Message}");
            return Enumerable.Empty<BauleiterWithProjectsDto>();
        }
    }

    public async Task<BauleiterWithProjectsDto?> UpdateBauleiter(BauleiterWithProjectsDto dto)
    {
        try
        {
            var bauleiterDto = new BauleiterDto
            {
                BauleiterId = dto.BauleiterId,
                Vorname = dto.Vorname,
                Nachname = dto.Nachname,
                Email = dto.Email,
                IsDeleted = dto.IsDeleted
            };
            var payload = new { Bauleiter = bauleiterDto };


            var response = await _httpClient.PutAsJsonAsync($"api/Bauleiter", payload);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<BauleiterWithProjectsDto>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new AccessTokenNotAvailableException(
                    _navigationManager,
                    null,
                    new[] { "api.write" }
                );
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to update Bauleiter. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateBauleiter: {ex.Message}");
            return null;
        }
    }
}
