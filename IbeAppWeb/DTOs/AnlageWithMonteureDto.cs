
namespace IbeAppWeb.DTOs;

public class AnlageWithMonteureDto
{
    public int AnlageId { get; set; }
    public string? AnlageName { get; set; }
    public string? Beschreibung { get; set; }
    public bool IsDeleted { get; set; }
    public List<MonteurResponse> Monteure { get; set; } = new List<MonteurResponse>();
}
