using IbeAppWeb.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace IbeAppWeb.Services;

public class MonteurService
{
    private readonly HttpClient _httpClient;

    public MonteurService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<MonteurResponse>> GetAllMonteure()
    {
        try
        {
            // Send a GET request to the API
            var response = await _httpClient.GetAsync("api/monteur");

            // Ensure the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Deserialize and return the list of AnlageDto
                return await response.Content.ReadFromJsonAsync<IEnumerable<MonteurResponse>>() ?? Enumerable.Empty<MonteurResponse>();
            }
            else
            {
                // Log or handle the error (optional)
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to fetch Monteure. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            // Log or rethrow the exception as needed
            Console.WriteLine($"Error in GetAllMonteure: {ex.Message}");
            return Enumerable.Empty<MonteurResponse>(); // Return an empty list on failure
        }
    }

    public async Task<List<MonteurWithAnlageDto>> GetMonteurWithAnlage()
    {
        try
        {
            // Send a GET request to the API
            var response = await _httpClient.GetAsync("api/monteur/with-anlagen");
            // Ensure the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Deserialize and return the list of AnlageDto
                return await response.Content.ReadFromJsonAsync<List<MonteurWithAnlageDto>>() ?? new List<MonteurWithAnlageDto>();
            }
            else
            {
                // Log or handle the error (optional)
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to fetch Monteure with Anlage. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            // Log or rethrow the exception as needed
            Console.WriteLine($"Error in GetMonteurWithAnlage: {ex.Message}");
            return new List<MonteurWithAnlageDto>(); // Return an empty list on failure
        }
    }

    public async Task<MonteurResponse?> CreateMonteur(MonteurResponse Dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/monteur", Dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<MonteurResponse>();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to create Monteur. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CreateMonteur: {ex.Message}");
            throw;
        }
    }

    public async Task<MonteurResponse?> UpdateMonteur(int Id, MonteurResponse Dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/monteur/{Id}", Dto);

            // Ensure the response is successful
            if (response.IsSuccessStatusCode)
            {
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
                        return JsonSerializer.Deserialize<MonteurResponse>(responseContent, options);
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
                    Console.WriteLine("Empty or whitespace response from the server.");
                    return null;
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to update Monteur. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateMonteur: {ex.Message}");
            throw;
        }
    }
    public async Task<bool> DeleteMonteur(int Id)
    {
        try
        {
            // Send a DELETE request to the API
            var response = await _httpClient.DeleteAsync($"api/monteur/{Id}");

            // Ensure the response is successful
            if (response.IsSuccessStatusCode)
            {
                return true; // Indicate success
            }
            else
            {
                // Log or handle the error (optional)
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to delete Monteur. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            // Log or rethrow the exception as needed
            Console.WriteLine($"Error in DeleteMonteur: {ex.Message}");
            throw;
        }
    }

    public async Task<MonteurWithAnlageDto?> AssignMonteurToAnlage(int monteurId, int anlageId)
    {
        try
        {
            // Format the URL with the MonteurId and AnlageId
            var url = $"api/monteur/{monteurId}/assign-to-anlage/{anlageId}";

            // Send a POST request to the API with the payload
            var response = await _httpClient.PostAsync(url, null);
            // Ensure the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Deserialize and return the response
                return await response.Content.ReadFromJsonAsync<MonteurWithAnlageDto>();
            }
            else
            {
                // Log or handle the error
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to assign Monteur to Anlage. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            // Log or rethrow the exception as needed
            Console.WriteLine($"Error in AssignMonteurToAnlage: {ex.Message}");
            throw;
        }
    }
}
