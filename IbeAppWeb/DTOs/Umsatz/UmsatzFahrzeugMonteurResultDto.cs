
namespace IbeAppWeb.DTOs.Umsatz;

public class UmsatzFahrzeugMonteurResultDto
{
    public List<UmsatzFahrzeugMonteurDto>? umsatzFahrzeugMonteurDtos { get; init; } = new ();
    public List<LinkDto> Links { get; init; } = new();
}
