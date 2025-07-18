using IbeAppWeb.DTOs;
using IbeAppWeb.DTOs.Monteur;
using IbeAppWeb.Validation;
using System.Net.Http.Json;
using System.Text.Json;

namespace IbeAppWeb.Services;

/// <summary>
/// Provides services for managing Monteur resources, including retrieval, creation, updating, and deletion operations.
/// </summary>
/// <remarks>This service interacts with a backend API to perform operations related to Monteur entities. It
/// handles HTTP requests and responses, including error handling and data serialization. Ensure that the <see
/// cref="HttpClient"/> is properly configured and the API endpoints are accessible.</remarks>

public class MonteurService
{
    private readonly HttpClient _httpClient;

    public MonteurService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Asynchronously retrieves a collection of all Monteure.
    /// </summary>
    /// <remarks>This method sends an HTTP GET request to the "api/monteur" endpoint to fetch the data. If the
    /// request is successful, it returns a collection of <see cref="MonteurResponse"/> objects. In case of a failure,
    /// an empty collection is returned.</remarks>
    /// <returns>A task representing the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/> of <see
    /// cref="MonteurResponse"/> objects representing the Monteure. Returns an empty collection if the request fails.</returns>
    /// <exception cref="HttpRequestException">Thrown if the HTTP request to fetch Monteure fails with a non-success status code.</exception>
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

    /// <summary>
    /// Asynchronously retrieves a list of Monteur with their associated Anlage data.
    /// </summary>
    /// <remarks>This method sends an HTTP GET request to the "api/monteur/with-anlagen" endpoint. If the
    /// request is successful, it returns a list of <see cref="MonteurWithAnlageDto"/> objects. If the request fails, it
    /// logs the error and returns an empty list.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see
    /// cref="MonteurWithAnlageDto"/> objects. If the request fails, the list will be empty.</returns>
    /// <exception cref="HttpRequestException">Thrown if the HTTP request to fetch Monteure with Anlage fails.</exception>
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

    /// <summary>
    /// Asynchronously creates a new Monteur by sending the provided data to the server.
    /// </summary>
    /// <remarks>If the server returns a Bad Request status, any validation errors are added to the <paramref
    /// name="fieldErrors"/> dictionary.</remarks>
    /// <param name="Dto">The <see cref="MonteurResponse"/> object containing the data to be sent for creating a new Monteur.</param>
    /// <param name="fieldErrors">A dictionary to store validation errors, if any, returned by the server. The keys are the field names, and the
    /// values are the corresponding error messages.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the created <see
    /// cref="MonteurResponse"/> if the operation is successful; otherwise, <see langword="null"/> if there are
    /// validation errors.</returns>
    /// <exception cref="HttpRequestException">Thrown if the request fails due to server errors or network issues.</exception>
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

    /// <summary>
    /// Updates the details of a specified Monteur using the provided data transfer object.
    /// </summary>
    /// <remarks>This method sends an HTTP PUT request to update the Monteur's details on the server. Ensure
    /// that the <paramref name="dto"/> contains valid and complete information for the Monteur to be updated.</remarks>
    /// <param name="dto">The <see cref="MonteurResponse"/> object containing the updated details of the Monteur.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated <see
    /// cref="MonteurResponse"/> if the update is successful; otherwise, <see langword="null"/>.</returns>
    /// <exception cref="HttpRequestException">Thrown if the HTTP request to update the Monteur fails.</exception>
    public async Task<MonteurResponse?> UpdateMonteur(MonteurResponse dto)
    {
        try
        {
            UpdateMonteurDto monteurUpdateDto = new UpdateMonteurDto
            {
                MonteurId = dto.MonteurId,
                Vorname = dto.Vorname,
                Nachname = dto.Nachname,
                Email = dto.Email,
                IsDeleted = dto.IsDeleted,
            };
            var payload = new { monteur = monteurUpdateDto };
            var response = await _httpClient.PutAsJsonAsync("api/monteur", payload);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<MonteurResponse>(responseContent, options);
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
            Console.WriteLine($"Error in AssignMonteurToAnlage: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Removes the assignment of an "Anlage" from a specified "Monteur".
    /// </summary>
    /// <param name="monteurId">The unique identifier of the "Monteur" from whom the "Anlage" assignment is to be removed.</param>
    /// <returns><see langword="true"/> if the assignment was successfully removed; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="HttpRequestException">Thrown if the HTTP request to remove the assignment fails, including details of the status code and error
    /// message.</exception>
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

    /// <summary>
    /// Retrieves the history of a specific Monteur Anlage based on the provided Monteur ID.
    /// </summary>
    /// <remarks>This method sends an asynchronous HTTP GET request to the specified API endpoint to retrieve
    /// the Monteur Anlage history. If the request is unsuccessful, an exception is thrown with details of the
    /// failure.</remarks>
    /// <param name="monteurId">The unique identifier of the Monteur whose Anlage history is to be retrieved.</param>
    /// <returns>A <see cref="MonteurAnlageHistoryDto"/> containing the history details if the request is successful; otherwise,
    /// <see langword="null"/>.</returns>
    /// <exception cref="HttpRequestException">Thrown if the HTTP request to fetch the Monteur Anlage history fails.</exception>
    public async Task<MonteurAnlageHistoryDto?> GetMonteurAnlageHistory(int monteurId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/monteurhistory/{monteurId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<MonteurAnlageHistoryDto>();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to fetch Monteur Anlage history. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetMonteurAnlageHistory: {ex.Message}");
            return null; 
        }
    }

    /// <summary>
    /// Asynchronously retrieves a list of Monteur projects.
    /// </summary>
    /// <remarks>This method sends an HTTP GET request to the specified API endpoint to fetch Monteur
    /// projects. If the request is successful, it returns a list of <see cref="ProjectMonteurDto"/> objects. In case of
    /// an error, an empty list is returned, and the error is logged to the console.</remarks>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of  <see
    /// cref="ProjectMonteurDto"/> objects representing the Monteur projects. Returns an empty list if the request
    /// fails.</returns>
    /// <exception cref="HttpRequestException">Thrown if the HTTP request to fetch Monteur projects fails with a non-success status code.</exception>
    public async Task<List<ProjectMonteurDto>> GetMonteurProjects()
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/monteur/projects");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ProjectMonteurDto>>() ?? new List<ProjectMonteurDto>();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to fetch Monteur projects. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetMonteurProjects: {ex.Message}");
            return new List<ProjectMonteurDto>(); 
        }
    }
}
