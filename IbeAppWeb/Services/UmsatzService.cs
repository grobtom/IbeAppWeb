using IbeAppWeb.DTOs;
using IbeAppWeb.DTOs.Umsatz;
using System.Globalization;
using System.Net.Http.Json;

namespace IbeAppWeb.Services;

public class UmsatzService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UmsatzService> _logger;

    public UmsatzService(HttpClient httpClient, ILogger<UmsatzService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<UmsatzResultDto> GetUmsatzByFahrzeugAndDateKanalAsync
    (
        DateTime? saniertAm = null,
        string projectDb = "defaultDb"
    )
    {
        try
        {
            var queryParameters = new List<string>();
            if (saniertAm.HasValue) queryParameters.Add($"fromDate={saniertAm.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");

            var queryString = string.Join("&", queryParameters);
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/umsatz/kanal/fahrzeug?{queryString}");
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
            // Build query parameters
            var queryParameters = new List<string>();
            if (start.HasValue) queryParameters.Add($"start={start.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
            if (end.HasValue) queryParameters.Add($"end={end.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
            var queryString = string.Join("&", queryParameters);

            HttpRequestMessage request;
            if(ComboboxValue == 1)
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
}
