
namespace IbeAppWeb.DTOs.Umsatz;

public class UmsatzFahrzeugMonteurDto
{
    public string PkLvPos { get; init; } = string.Empty;
    public string Kurztext { get; init; } = string.Empty;
    public string ProjectName { get; init; }
    public DateTime Ausfuehrungsdatum { get; init; }
    public string Fahrzeug { get; init; }
    public string Kolonnenfuehrer { get; init; }
    public decimal Gesamtumsatz { get; init; }
}
