using IbeAppWeb.DTOs;
using System.Net.Http.Json;

namespace IbeAppWeb.Services;

public class BauleiterService
{
    private readonly HttpClient _httpClient;

    public BauleiterService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
        public async Task<IEnumerable<BauleiterDto>> GetAllBauleiter()
        {
            try
            {
                // Send a GET request to the API
                var response = await _httpClient.GetAsync("api/Bauleiter");
    
                // Ensure the response is successful
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize and return the list of BauleiterDto
                    return await response.Content.ReadFromJsonAsync<IEnumerable<BauleiterDto>>() ?? Enumerable.Empty<BauleiterDto>();
                }
                else
                {
                    // Log or handle the error (optional)
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Failed to fetch Bauleiters. Status: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                // Log or rethrow the exception as needed
                Console.WriteLine($"Error in GetAllBauleiters: {ex.Message}");
                return Enumerable.Empty<BauleiterDto>(); // Return an empty list on failure
            }
    }

    public async Task<IEnumerable<BauleiterWithProjectsDto>> GetBauleiterWithProjects()
    {
        try
        {
            // Send a GET request to the API
            var response = await _httpClient.GetAsync("api/Bauleiter/with-projects");
            // Ensure the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Deserialize and return the list of BauleiterWithProjectsDto
                return await response.Content.ReadFromJsonAsync<IEnumerable<BauleiterWithProjectsDto>>() ?? Enumerable.Empty<BauleiterWithProjectsDto>();
            }
            else
            {
                // Log or handle the error (optional)
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to fetch Bauleiters with projects. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            // Log or rethrow the exception as needed
            Console.WriteLine($"Error in GetBauleiterWithProjects: {ex.Message}");
            return Enumerable.Empty<BauleiterWithProjectsDto>(); // Return an empty list on failure
        }
    }
}
