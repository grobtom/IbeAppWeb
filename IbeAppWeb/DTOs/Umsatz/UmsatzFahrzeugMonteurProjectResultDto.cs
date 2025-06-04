
namespace IbeAppWeb.DTOs.Umsatz;

public class UmsatzFahrzeugMonteurProjectResultDto
{
    public List<UmsatzFahrzeugMonteurProjectDto>? Projects { get; init; } = new();
    public List<LinkDto>? Links { get; init; } = new();
}
