using IbeAppWeb.DTOs;
using IbeAppWeb.DTOs.Umsatz;
using System.Globalization;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

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

            if(ComboBoxValue == 1)
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
