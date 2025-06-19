namespace IbeAppWeb.DTOs.Monteur;

public class MonteurResponse
{
    public int MonteurId { get; set; }
    public string? Vorname { get; set; }
    public string? Nachname { get; set; }
    public string? Email { get; set; }
    public bool IsDeleted { get; set; } = false;
    public int? AnlageId { get; set; }
    public int Order { get; set; }
    public int Sequence { get; set; } = 1;
    public string FullName => string.Join(" ", new[] { Nachname?.Trim(), Vorname?.Trim() }.Where(s => !string.IsNullOrWhiteSpace(s))).Trim();
}