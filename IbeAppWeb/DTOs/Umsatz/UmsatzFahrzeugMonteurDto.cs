
namespace IbeAppWeb.DTOs.Umsatz;

public class UmsatzFahrzeugMonteurDto
{
    public string ProjectName { get; init; }
    public DateTime Ausfuehrungsdatum { get; init; }
    public string Typ { get; init; }
    public string Ressource { get; init; }
    public string Kolonnenfuehrer { get; init; }
    public decimal Gesamtumsatz { get; init; }
}
