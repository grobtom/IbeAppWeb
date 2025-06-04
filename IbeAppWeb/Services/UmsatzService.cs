using IbeAppWeb.DTOs;
using IbeAppWeb.DTOs.Umsatz;
using System.Globalization;
using System.Net.Http.Json;

namespace IbeAppWeb.Services;

/// <summary>
/// Provides methods for retrieving revenue data (Umsatz) based on various criteria, such as vehicle,  date ranges, and
/// project database context.
/// </summary>
/// <remarks>This service interacts with an external API to fetch revenue-related data. It supports multiple 
/// query scenarios, including filtering by vehicle, date ranges, and project-specific contexts.  The service handles
/// HTTP requests and responses, ensuring proper error logging and default  fallback values in case of
/// failures.</remarks>

public class UmsatzService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UmsatzService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UmsatzService"/> class.
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> instance used to send HTTP requests.</param>
    /// <param name="logger">The <see cref="ILogger{UmsatzService}"/> instance used for logging operations.</param>

    public UmsatzService(HttpClient httpClient, ILogger<UmsatzService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves revenue data for a vehicle based on the specified date and context.
    /// </summary>
    /// <remarks>This method sends an HTTP request to fetch revenue data for a vehicle. The context of the
    /// query is determined by the <paramref name="ComboBoxValue"/> parameter. Ensure that the <paramref
    /// name="projectDb"/> parameter matches the name of a valid project database.</remarks>
    /// <param name="saniertAm">The date to filter the revenue data. If <see langword="null"/>, no date filter is applied.</param>
    /// <param name="ComboBoxValue">Determines the context for the query. Use <see langword="1"/> for "kanal" context and any other value for
    /// "schacht" context.</param>
    /// <param name="projectDb">The name of the project database to use for the query. Defaults to "defaultDb" if not specified.</param>
    /// <returns>An <see cref="UmsatzResultDto"/> containing the revenue data for the specified vehicle and date. Returns an
    /// empty <see cref="UmsatzResultDto"/> if an error occurs during the operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the response content is <see langword="null"/>.</exception>

    public async Task<UmsatzResultDto> GetUmsatzByFahrzeugAndDateAsync
    (
        DateTime? saniertAm = null,
        int ComboBoxValue = 1,
        string projectDb = "defaultDb"
    )
    {
        try
        {
            var queryParameters = new List<string>();
            if (saniertAm.HasValue) queryParameters.Add($"atDate={saniertAm.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");

            var queryString = string.Join("&", queryParameters);

            HttpRequestMessage request;

            if (ComboBoxValue == 1)
            {
                request = new HttpRequestMessage(HttpMethod.Get, $"api/umsatz/kanal/fahrzeug?{queryString}");
            }
            else
            {
                request = new HttpRequestMessage(HttpMethod.Get, $"api/umsatz/schacht/fahrzeug?{queryString}");
            }

            request.Headers.Add("X-IbeProjectDB", projectDb);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<UmsatzResultDto>();
            if (result == null)
            {
                throw new InvalidOperationException("Response content is null.");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching Umsatzdatá");
            return new UmsatzResultDto();
        }
    }

    /// <summary>
    /// Retrieves revenue data for a vehicle mechanic based on the specified date range, category, and project database.
    /// </summary>
    /// <remarks>This method sends an HTTP request to fetch revenue data for a vehicle mechanic. The data can
    /// be filtered by a date range and category, and the target database can be specified. If no date range is
    /// provided, the method retrieves all available data. The category is determined by the <paramref
    /// name="ComboBoxValue"/> parameter, which selects between "kanal" (value 1) and "schacht" (other
    /// values).</remarks>
    /// <param name="SaniertAmVon">The start date of the date range filter. If <see langword="null"/>, no start date filter is applied.</param>
    /// <param name="SaniertAmBis">The end date of the date range filter. If <see langword="null"/>, no end date filter is applied.</param>
    /// <param name="ComboBoxValue">The category selector. Use 1 for "kanal" and any other value for "schacht". Defaults to 1.</param>
    /// <param name="projectDb">The name of the project database to query. Defaults to "defaultDb".</param>
    /// <returns>An <see cref="UmsatzFahrzeugMonteurResultDto"/> containing the revenue data for the specified filters. If an
    /// error occurs or the response content is <see langword="null"/>, an empty result is returned.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the response content is <see langword="null"/> after a successful HTTP request.</exception>

    public async Task<UmsatzFahrzeugMonteurResultDto> GetUmsatzByFahrzeugMonteurAsync
    (
        DateTime? SaniertAmVon = null,
        DateTime? SaniertAmBis = null,
        int ComboBoxValue = 1,
        string projectDb = "defaultDb"
    )
    {
        try
        {
            var queryParameters = new List<string>();
            if (SaniertAmVon.HasValue) queryParameters.Add($"start={SaniertAmVon.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
            if (SaniertAmBis.HasValue) queryParameters.Add($"end={SaniertAmBis.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
            var queryString = string.Join("&", queryParameters);
            HttpRequestMessage request;
            if (ComboBoxValue == 1)
            {
                request = new HttpRequestMessage(HttpMethod.Get, $"api/umsatz/kanal/fahrzeugmonteur?{queryString}");
            }
            else
            {
                request = new HttpRequestMessage(HttpMethod.Get, $"api/umsatz/schacht/fahrzeugmonteur?{queryString}");
            }
            request.Headers.Add("X-IbeProjectDB", projectDb);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<UmsatzFahrzeugMonteurResultDto>();
            if (result == null)
            {
                throw new InvalidOperationException("Response content is null.");
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching Umsatz data");
            return new UmsatzFahrzeugMonteurResultDto();
        }
    }

    /// <summary>
    /// Retrieves revenue data for vehicles and mechanics within a specified date range and project context.
    /// </summary>
    /// <remarks>This method fetches revenue data based on the provided parameters, including the date range,
    /// project database, and a combobox value that determines the type of query. The data is retrieved from an external
    /// API and returned as a <see cref="UmsatzFahrzeugMonteurProjectResultDto"/> object. If no date range is specified,
    /// the method retrieves data for all available dates.</remarks>
    /// <param name="start">The start date of the revenue data range. If <see langword="null"/>, no start date filter is applied.</param>
    /// <param name="end">The end date of the revenue data range. If <see langword="null"/>, no end date filter is applied.</param>
    /// <param name="ComboboxValue">Determines the type of query to execute. A value of 1 retrieves data for "kanal" (channel), while other values
    /// retrieve data for "schacht" (shaft).</param>
    /// <param name="projectDb">The name of the project database to query. Defaults to "defaultDb" if not specified.</param>
    /// <returns>A <see cref="UmsatzFahrzeugMonteurProjectResultDto"/> object containing the revenue data for vehicles and
    /// mechanics. Returns an empty result if an error occurs during the API call.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the response content from the API is <see langword="null"/>.</exception>

    public async Task<UmsatzFahrzeugMonteurProjectResultDto> GetUmsatzByFahrzeugAndMonteurAllAsync
    (
        DateTime? start = null,
        DateTime? end = null,
        int ComboboxValue = 1,
        string projectDb = "defaultDb"
    )
    {
        try
        {
            var queryParameters = new List<string>();
            if (start.HasValue) queryParameters.Add($"start={start.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
            if (end.HasValue) queryParameters.Add($"end={end.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
            var queryString = string.Join("&", queryParameters);

            HttpRequestMessage request;
            if (ComboboxValue == 1)
            {
                request = new HttpRequestMessage(HttpMethod.Get, $"api/umsatz/kanal/fahrzeugmonteur/project?{queryString}");
            }
            else
            {
                request = new HttpRequestMessage(HttpMethod.Get, $"api/umsatz/schacht/fahrzeugmonteur/project?{queryString}");
            }

            // Send the request
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // Deserialize the response
            var result = await response.Content.ReadFromJsonAsync<UmsatzFahrzeugMonteurProjectResultDto>();
            if (result == null)
            {
                throw new InvalidOperationException("Response content is null.");
            }

            return result;
        }
        catch (Exception ex)
        {
            // Log the error and return an empty result
            _logger.LogError(ex, "Error fetching Umsatz data");
            return new UmsatzFahrzeugMonteurProjectResultDto();
        }
    }

    /// <summary>
    /// Retrieves the revenue split for all projects within the specified date range.
    /// </summary>
    /// <remarks>This method sends an HTTP GET request to the API endpoint to fetch the revenue split data.
    /// Ensure that the HTTP client is properly configured and the API endpoint is accessible.</remarks>
    /// <param name="startDatum">The start date of the range for which the revenue split is calculated. If <see langword="null"/>, no start date
    /// filter is applied.</param>
    /// <param name="endDatum">The end date of the range for which the revenue split is calculated. If <see langword="null"/>, no end date
    /// filter is applied.</param>
    /// <returns>An <see cref="AllProjectsUmsatzSplitDto"/> object containing the revenue split data for all projects. If an
    /// error occurs, an empty <see cref="AllProjectsUmsatzSplitDto"/> is returned.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the response content is <see langword="null"/>.</exception>

    public async Task<AllProjectsUmsatzSplitDto> GetProjectsUmsatzSplit(DateTime? startDatum, DateTime? endDatum)
    {
        try
        {
            var queryParameters = new List<string>();
            if (startDatum.HasValue) queryParameters.Add($"startDatum={startDatum.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
            if (endDatum.HasValue) queryParameters.Add($"endDatum={endDatum.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
            var queryString = string.Join("&", queryParameters);

            HttpRequestMessage request;
            request = new HttpRequestMessage(HttpMethod.Get, $"api/umsatz/all-projects-umsatz?{queryString}");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // Deserialize the response
            var result = await response.Content.ReadFromJsonAsync<AllProjectsUmsatzSplitDto>();
            if (result == null)
            {
                throw new InvalidOperationException("Response content is null.");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching AllProjectsUmsatzSplit");
            return new AllProjectsUmsatzSplitDto();
        }
    }
}
