using IbeAppWeb.DTOs;
using IbeAppWeb.Validation;
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
            var response = await _httpClient.GetAsync("api/monteur");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<MonteurResponse>>() ?? Enumerable.Empty<MonteurResponse>();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to fetch Monteure. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllMonteure: {ex.Message}");
            return Enumerable.Empty<MonteurResponse>(); // Return an empty list on failure
        }
    }

    public async Task<List<MonteurWithAnlageDto>> GetMonteurWithAnlage()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/monteur/with-anlagen");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<MonteurWithAnlageDto>>() ?? new List<MonteurWithAnlageDto>();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to fetch Monteure with Anlage. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetMonteurWithAnlage: {ex.Message}");
            return new List<MonteurWithAnlageDto>(); // Return an empty list on failure
        }
    }

    public async Task<MonteurResponse?> CreateMonteur(MonteurResponse Dto, Dictionary<string, string> fieldErrors)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/monteur", Dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<MonteurResponse>();
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

    /// <summary>
    /// Deletes a Monteur resource identified by the specified ID.
    /// </summary>
    /// <remarks>This method sends an HTTP DELETE request to the server to remove the specified Monteur.
    /// Ensure the provided ID is valid and the server is reachable.</remarks>
    /// <param name="Id">The unique identifier of the Monteur to delete. Must be a positive integer.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the deletion is
    /// successful; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="HttpRequestException">Thrown if the HTTP request to delete the Monteur fails, including details about the status code and error
    /// content.</exception>

    public async Task<bool> DeleteMonteur(int Id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/monteur/{Id}");

            if (response.IsSuccessStatusCode)
            {
                return true; 
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to delete Monteur. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteMonteur: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Assigns a Monteur to an Anlage based on the provided data transfer object (DTO).
    /// </summary>
    /// <remarks>This method sends an HTTP POST request to the endpoint <c>api/monteur/assign-to-anlage</c>
    /// with the provided DTO as the request body. Ensure that the <see cref="AssignMonteurToAnlageDto"/> object is
    /// properly populated before calling this method.</remarks>
    /// <param name="dto">The data transfer object containing the details required to assign a Monteur to an Anlage.</param>
    /// <returns>A <see cref="MonteurWithAnlageDto"/> object containing the details of the assigned Monteur and Anlage if the
    /// operation is successful; otherwise, <see langword="null"/>.</returns>
    /// <exception cref="HttpRequestException">Thrown if the HTTP request to assign the Monteur to the Anlage fails. The exception message includes the HTTP
    /// status code and error details.</exception>

    public async Task<MonteurWithAnlageDto?> AssignMonteurToAnlage(AssignMonteurToAnlageDto dto)
    {
        try
        {
            var content = JsonContent.Create(dto);

            var response = await _httpClient.PostAsync("api/monteur/assign-to-anlage", content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<MonteurWithAnlageDto>();
            }
            else
            {
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

    public async Task<bool> RemoveAnlageFromMonteur(int monteurId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/monteur/remove-assignment/{monteurId}");
            if (response.IsSuccessStatusCode)
            {
                return true; 
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to remove Anlage from Monteur. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in RemoveAnlageFromMonteur: {ex.Message}");
            throw;
        }
    }
}
