using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Globalization;
using IbeAppWeb.DTOs;

public class ArbeitsscheinService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ArbeitsscheinService> _logger;

    public ArbeitsscheinService(HttpClient httpClient, ILogger<ArbeitsscheinService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<ArbeitsscheinDto>> GetArbeitsscheineAsync(string? firma = null, DateTime? saniertAmVon = null, DateTime? saniertAmBis = null, DateTime? saniertAm = null, string? abschlagsrechnung = null, string? kolonnenfuehrer = null, string? fahrzeug = null, string projectDb = "defaultDb")
    {
        try
        {
            var queryParameters = new List<string>();
            if (!string.IsNullOrEmpty(firma)) queryParameters.Add($"Firma={firma}");
            if (saniertAmVon.HasValue) queryParameters.Add($"SaniertAmVon={saniertAmVon.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
            if (saniertAmBis.HasValue) queryParameters.Add($"SaniertAmBis={saniertAmBis.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
            if (saniertAm.HasValue) queryParameters.Add($"SaniertAm={saniertAm.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
            if (!string.IsNullOrEmpty(abschlagsrechnung)) queryParameters.Add($"Abschlagsrechnung={abschlagsrechnung}");
            if (!string.IsNullOrEmpty(kolonnenfuehrer)) queryParameters.Add($"Kolonnenfuehrer={kolonnenfuehrer}");
            if (!string.IsNullOrEmpty(fahrzeug)) queryParameters.Add($"Fahrzeug={fahrzeug}");

            var queryString = string.Join("&", queryParameters);
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/askanal?{queryString}");
            request.Headers.Add("X-IbeProjectDB", projectDb);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<ArbeitsscheinDto>>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching Arbeitsscheine");
            return new List<ArbeitsscheinDto>();
        }
    }

    public async Task<List<ArbeitsscheinDto>> GetArbeitsscheineSchachtAsync(string? firma = null, DateTime? saniertAmVon = null, DateTime? saniertAmBis = null, DateTime? saniertAm = null, string? abschlagsrechnung = null, string? kolonnenfuehrer = null, string? fahrzeug = null, string projectDb = "defaultDb")
    {
        try
        {
            var queryParameters = new List<string>();
            if (!string.IsNullOrEmpty(firma)) queryParameters.Add($"Firma={firma}");
            if (saniertAmVon.HasValue) queryParameters.Add($"SaniertAmVon={saniertAmVon.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
            if (saniertAmBis.HasValue) queryParameters.Add($"SaniertAmBis={saniertAmBis.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
            if (saniertAm.HasValue) queryParameters.Add($"SaniertAm={saniertAm.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
            if (!string.IsNullOrEmpty(abschlagsrechnung)) queryParameters.Add($"Abschlagsrechnung={abschlagsrechnung}");
            if (!string.IsNullOrEmpty(kolonnenfuehrer)) queryParameters.Add($"Kolonnenfuehrer={kolonnenfuehrer}");
            if (!string.IsNullOrEmpty(fahrzeug)) queryParameters.Add($"Fahrzeug={fahrzeug}");

            var queryString = string.Join("&", queryParameters);
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/asschacht?{queryString}");
            request.Headers.Add("X-IbeProjectDB", projectDb);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<ArbeitsscheinDto>>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching Arbeitsscheine Schacht");
            return new List<ArbeitsscheinDto>();
        }
    }
}

