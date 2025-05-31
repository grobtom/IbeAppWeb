using System.Net.Http.Json;
using System.Text.Json;
using IbeAppWeb.DTOs;

namespace IbeAppWeb.Services;

public class AnlagenService
{
    private readonly HttpClient _httpClient;

    public AnlagenService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<AnlageDto>> GetAllAnlagen()
    {
        try
        {
            // Send a GET request to the API
            var response = await _httpClient.GetAsync("api/Anlage");

            // Ensure the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Deserialize and return the list of AnlageDto
                return await response.Content.ReadFromJsonAsync<IEnumerable<AnlageDto>>() ?? Enumerable.Empty<AnlageDto>();
            }
            else
            {
                // Log or handle the error (optional)
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to fetch Anlagen. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            // Log or rethrow the exception as needed
            Console.WriteLine($"Error in GetAllAnlagen: {ex.Message}");
            return Enumerable.Empty<AnlageDto>(); // Return an empty list on failure
        }
    }

    public async Task<IEnumerable<AnlageWithMonteureDto>> GetAllAnlagenWithMonteure()
    {
        try
        {
            // Send a GET request to the API
            var response = await _httpClient.GetAsync("api/Anlage/withmonteure");

            // Ensure the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Deserialize and return the list of AnlageDto
                return await response.Content.ReadFromJsonAsync<IEnumerable<AnlageWithMonteureDto>>() ?? Enumerable.Empty<AnlageWithMonteureDto>();
            }
            else
            {
                // Log or handle the error (optional)
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to fetch Anlagen. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            // Log or rethrow the exception as needed
            Console.WriteLine($"Error in GetAllAnlagen: {ex.Message}");
            return Enumerable.Empty<AnlageWithMonteureDto>(); // Return an empty list on failure
        }
    }

    public async Task<AnlageWithMonteureDto> GetAnlageWithMonteureById(int anlageId)
    {
        try
        {
            // Send a GET request to the API
            var response = await _httpClient.GetAsync($"api/Anlage/withmonteure/{anlageId}");
            // Ensure the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Deserialize and return the AnlageWithMonteureDto
                return await response.Content.ReadFromJsonAsync<AnlageWithMonteureDto>() ?? new AnlageWithMonteureDto();
            }
            else
            {
                // Log or handle the error (optional)
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to fetch Anlage with Monteure by ID. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            // Log or rethrow the exception as needed
            Console.WriteLine($"Error in GetAnlageWithMonteureById: {ex.Message}");
            return new AnlageWithMonteureDto(); // Return an empty object on failure
        }
    }

    public async Task<AnlageDto?> GetAnlageById(int anlageId)
    {
        try
        {
            // Send a GET request to the API
            var response = await _httpClient.GetAsync($"api/Anlage/{anlageId}");

            // Ensure the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Deserialize and return the AnlageDto
                return await response.Content.ReadFromJsonAsync<AnlageDto>();
            }
            else
            {
                // Log or handle the error (optional)
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to fetch Anlage by ID. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            // Log or rethrow the exception as needed
            Console.WriteLine($"Error in GetAnlageById: {ex.Message}");
            return null; // Return null on failure
        }
    }

    public async Task<AnlageDto?> CreateAnlage(string anlageName, string beschreibung)
    {
        try
        {
            // Trim input to avoid trailing/leading spaces
            var trimmedName = anlageName?.Trim();
            var trimmedBeschreibung = beschreibung?.Trim();

            // Validate input
            if (string.IsNullOrWhiteSpace(trimmedName))
                throw new ArgumentException("AnlageName darf nicht leer sein.");

            // Build the query string
            var url = $"api/Anlage?anlagename={Uri.EscapeDataString(trimmedName)}&beschreibung={Uri.EscapeDataString(trimmedBeschreibung ?? string.Empty)}";

            // Send a POST request with no body, parameters in query string
            var response = await _httpClient.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AnlageDto>();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"CreateAnlage failed: {errorContent}");
                throw new HttpRequestException($"Failed to create Anlage. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CreateAnlage: {ex.Message}");
            throw;
        }
    }

    public async Task<AnlageDto?> UpdateAnlage(int anlageId, AnlageDto anlageDto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/anlage/{anlageId}", anlageDto);

            if (response.IsSuccessStatusCode)
            {
                // Expecting a 200 OK with AnlageDto in the body
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Raw JSON response: {responseContent}");

                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    try
                    {
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        return JsonSerializer.Deserialize<AnlageDto>(responseContent, options);
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine($"Deserialization error: {jsonEx.Message}");
                        Console.WriteLine($"Raw response content: {responseContent}");
                        return null;
                    }
                }
                else
                {
                    // If the server returns 204 NoContent, return null
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return null;
                    }
                    Console.WriteLine("Empty or whitespace response from the server.");
                    return null;
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to update Anlage. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAnlage: {ex.Message}");
            throw;
        }
    }
    public async Task<bool> DeleteAnlage(int anlageId)
    {
        try
        {
            // Send a DELETE request to the API
            var response = await _httpClient.DeleteAsync($"api/Anlage/{anlageId}");

            // Ensure the response is successful
            if (response.IsSuccessStatusCode)
            {
                return true; // Indicate success
            }
            else
            {
                // Log or handle the error (optional)
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to delete Anlage. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            // Log or rethrow the exception as needed
            Console.WriteLine($"Error in DeleteAnlage: {ex.Message}");
            throw;
        }
    }

    public async Task<AnlageDto?> UndeleteAnlage(int anlageId)
    {
        try
        {
            // Send a PUT request to the undelete endpoint
            var response = await _httpClient.PutAsJsonAsync($"api/Anlage/{anlageId}/undelete", new { });

            // Ensure the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Deserialize and return the updated AnlageDto
                return await response.Content.ReadFromJsonAsync<AnlageDto>();
            }
            else
            {
                // Log or handle the error (optional)
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to undelete Anlage. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            // Log or rethrow the exception as needed
            Console.WriteLine($"Error in UndeleteAnlage: {ex.Message}");
            throw;
        }
    }
}
