using IbeAppWeb.DTOs;
using System.Net.Http.Json;

namespace IbeAppWeb.Services;

/// <summary>
/// Provides functionality for retrieving invoice data related to projects.
/// </summary>
/// <remarks>This service is designed to interact with an external API to fetch invoice information. It supports
/// fetching invoices for different project types, such as "kanal" or "schacht".</remarks>

public class RechnungService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProjectService> _logger;

    public RechnungService(HttpClient httpClient, ILogger<ProjectService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a list of AbschlagsRechnungDto objects from the specified project database.
    /// </summary>
    /// <remarks>This method sends an HTTP GET request to the appropriate API endpoint based on the value of
    /// <paramref name="isSchacht"/>.  The <paramref name="projectDb"/> parameter is included as a custom header
    /// ("X-IbeProjectDB") in the request. If the request fails or an exception is thrown, the method logs the error and
    /// returns an empty list.</remarks>
    /// <param name="projectDb">The name of the project database to query. This value is sent as a header in the request.</param>
    /// <param name="isSchacht">A boolean value indicating whether to fetch Schacht-related data.  If <see langword="true"/>, the method
    /// retrieves Schacht-related invoices; otherwise, Kanal-related invoices.</param>
    /// <returns>A task representing the asynchronous operation. The result contains a list of <see cref="AbschlagsRechnungDto"/>
    /// objects  if the request is successful, or an empty list if an error occurs.</returns>
    public async Task<List<AbschlagsRechnungDto>?> GetAbschlagsRechnungenAsync(string projectDb, int comboBoxValue)
    {
        try
        {
            HttpRequestMessage request;
            if (comboBoxValue == 1) 
            {
                request = new HttpRequestMessage(HttpMethod.Get, $"api/rechnung/kanal/all");
            }
            else
            {
                request = new HttpRequestMessage(HttpMethod.Get, $"api/rechnung/schacht/all");
            }
            request.Headers.Add("X-IbeProjectDB", projectDb);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<AbschlagsRechnungDto>>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching Abschlagsrechnungen");
            return new List<AbschlagsRechnungDto>();
        }
    }

    /// <summary>
    /// Updates the Kanal Abschlagsrechnung or Schacht Abschlagsrechnung for a specified date range.
    /// </summary>
    /// <remarks>This method sends an HTTP POST request to update Abschlagsrechnung data based on the
    /// specified parameters. Ensure that the <paramref name="projectDb"/> and <paramref name="abschlagsrechnung"/>
    /// values are valid.</remarks>
    /// <param name="projectDb">The name of the project database to use for the operation. Cannot be null or empty.</param>
    /// <param name="startDate">The start date of the range for which the Abschlagsrechnung should be updated.</param>
    /// <param name="endDate">The end date of the range for which the Abschlagsrechnung should be updated.</param>
    /// <param name="abschlagsrechnung">The identifier or type of the Abschlagsrechnung to update. Cannot be null or empty.</param>
    /// <param name="comboBoxValue">Determines the type of Abschlagsrechnung to update.  Use <see langword="1"/> for Kanal Abschlagsrechnung and any
    /// other value for Schacht Abschlagsrechnung.</param>
    /// <returns>The number of records updated as an integer. Returns <see langword="-1"/> if an error occurs during the
    /// operation.</returns>
    public async Task<int> UpdateKanalAbschlagsrechnungByDateAsync(string projectDb, DateTime startDate, DateTime endDate, string abschlagsrechnung, int comboBoxValue)
    {
        try
        {
            var command = new
            {
                StartDate = startDate,
                EndDate = endDate,
                Abschlagsrechnung = abschlagsrechnung
            };

            HttpRequestMessage request;

            if (comboBoxValue == 1)
            {
                request = new HttpRequestMessage(HttpMethod.Post, "api/rechnung/kanal/abschlag/update/date");
            }
            else
            { 
                request = new HttpRequestMessage(HttpMethod.Post, "api/rechnung/schacht/abschlag/Update/date");
            }

            request.Headers.Add("X-IbeProjectDB", projectDb);
            request.Content = JsonContent.Create(command);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadFromJsonAsync<int>();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating Kanal Abschlagsrechnung by date");
            return -1;
        }
    }

    /// <summary>
    /// Updates the Kanal Abschlagsrechnung or Schacht Abschlagsrechnung for a specified date range.
    /// </summary>
    /// <remarks>This method sends an HTTP POST request to update Abschlagsrechnung data based on the
    /// specified parameters. Ensure that the <paramref name="projectDb"/> and <paramref name="abschlagsrechnung"/>
    /// values are valid.</remarks>
    /// <param name="projectDb">The name of the project database to use for the operation. Cannot be null or empty.</param>
    /// <param name="startDate">The start date of the range for which the Abschlagsrechnung should be updated.</param>
    /// <param name="endDate">The end date of the range for which the Abschlagsrechnung should be updated.</param>
    /// <param name="abschlagsrechnung">The identifier or type of the Abschlagsrechnung to update. Cannot be null or empty.</param>
    /// <param name="comboBoxValue">Determines the type of Abschlagsrechnung to update.  Use <see langword="1"/> for Kanal Abschlagsrechnung and any
    /// other value for Schacht Abschlagsrechnung.</param>
    /// <returns>The number of records updated as an integer. Returns <see langword="-1"/> if an error occurs during the
    /// operation.</returns>
    public async Task<int> UpdateKanalAbschlagsrechnungByIdsAsync(string projectDb, int idFrom, int idTo, string abschlagsrechnung, int comboBoxValue)
    {
        try
        {
            var command = new
            {
                startId = idFrom,
                endId = idTo,
                Abschlagsrechnung = abschlagsrechnung
            };

            HttpRequestMessage request;

            if (comboBoxValue == 1)
            {
                request = new HttpRequestMessage(HttpMethod.Post, "api/rechnung/kanal/abschlag/update/id");
            }
            else
            {
                request = new HttpRequestMessage(HttpMethod.Post, "api/rechnung/schacht/abschlagupdate/id");
            }

            request.Headers.Add("X-IbeProjectDB", projectDb);
            request.Content = JsonContent.Create(command);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<int>();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating Kanal Abschlagsrechnung by ids");
            return -1;
        }
    }

}
