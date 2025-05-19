
namespace IbeAppWeb.DTOs.Umsatz;

public class UmsatzFahrzeugMonteurProjectDto
{
    public Project Project { get; set; } = new();
    public List<UmsatzFahrzeugMonteurDto> UmsatzProject { get; set; } = new();
    public decimal TotalUmsatz { get; set; }
}
