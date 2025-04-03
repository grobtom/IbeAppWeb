using IbeAppWeb.DTOs;
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

    public async Task<List<UmsatzDto>> GetUmsatzByFahrzeugAndDateKanalAsync
    (
        DateTime? saniertAm = null, 
        string? fahrzeug = null, 
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

            return await response.Content.ReadFromJsonAsync<List<UmsatzDto>>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching Umsatzdatá");
            return new List<UmsatzDto>();
        }
    }
}
