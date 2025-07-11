using IbeAppWeb.DTOs;
using IbeAppWeb.Validation;
using System.Net.Http.Json;
using System.Text.Json;

namespace IbeAppWeb.Services;

/// <summary>
/// Provides methods for managing "Anlage" entities, including retrieval, creation, updating, deletion,  and
/// restoration. This service communicates with a backend API to perform operations on "Anlage" data.
/// </summary>
/// <remarks>The <see cref="AnlagenService"/> class is designed to interact with a RESTful API for managing
/// "Anlage"  entities. It provides methods to retrieve all "Anlage" records, retrieve specific records by ID,  create
/// new records, update existing ones, delete records, and restore previously deleted records.  Each method handles HTTP
/// communication and deserialization of API responses.  Exceptions are thrown for HTTP errors or unexpected conditions,
/// and some methods return default values  (e.g., empty collections or null) in case of failure. Callers should handle
/// exceptions and validate  return values as needed.</remarks>

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
            var response = await _httpClient.GetAsync("api/Anlage");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<AnlageDto>>() ?? Enumerable.Empty<AnlageDto>();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to fetch Anlagen. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllAnlagen: {ex.Message}");
            return Enumerable.Empty<AnlageDto>(); // Return an empty list on failure
        }
    }

    public async Task<IEnumerable<AnlageDto>> GetProjectAnlagen(int comboboxValue, string projectDb)
    {
        try
        {
            HttpRequestMessage request;
            if (comboboxValue == 1)
                {
                request = new HttpRequestMessage(HttpMethod.Get, "api/mhzlh/anlagen");
            }
            else
            {
                request = new HttpRequestMessage(HttpMethod.Get, $"api/mkzlh/anlagen");                }
            request.Headers.Add("X-IbeProjectDB", projectDb);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<AnlageDto>>() ?? Enumerable.Empty<AnlageDto>();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to fetch Anlagen. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetProjectAnlagen: {ex.Message}");
            return Enumerable.Empty<AnlageDto>(); 
        }
    }

    public async Task<IEnumerable<AnlageWithMonteureDto>> GetAllAnlagenWithMonteure()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Anlage/withmonteure");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<AnlageWithMonteureDto>>() ?? Enumerable.Empty<AnlageWithMonteureDto>();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to fetch Anlagen. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllAnlagen: {ex.Message}");
            return Enumerable.Empty<AnlageWithMonteureDto>(); // Return an empty list on failure
        }
    }

    public async Task<AnlageWithMonteureDto> GetAnlageWithMonteureById(int anlageId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Anlage/withmonteure/{anlageId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AnlageWithMonteureDto>() ?? new AnlageWithMonteureDto();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to fetch Anlage with Monteure by ID. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAnlageWithMonteureById: {ex.Message}");
            return new AnlageWithMonteureDto(); // Return an empty object on failure
        }
    }

    public async Task<AnlageDto?> GetAnlageById(int anlageId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Anlage/{anlageId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AnlageDto>();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to fetch Anlage by ID. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAnlageById: {ex.Message}");
            return null; // Return null on failure
        }
    }

    public async Task<AnlageDto?> CreateAnlage(AnlageDto anlageDto, Dictionary<string, string> fieldErrors)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/anlage", anlageDto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AnlageDto>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var error = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();

                if (error?.Errors != null)
                {
                    foreach (var item in error.Errors)
                    {
                        fieldErrors[item.PropertyName] = item.ErrorMessage;
                    }
                }

                return null;
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
            var response = await _httpClient.DeleteAsync($"api/Anlage/{anlageId}");

            if (response.IsSuccessStatusCode)
            {
                return true; 
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to delete Anlage. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteAnlage: {ex.Message}");
            throw;
        }
    }

    public async Task<AnlageDto?> UndeleteAnlage(int anlageId)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Anlage/{anlageId}/undelete", new { });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AnlageDto>();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to undelete Anlage. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UndeleteAnlage: {ex.Message}");
            throw;
        }
    }
}
