namespace IbeAppWeb.DTOs;
public class BauleiterDto
{
    public int BauleiterId { get; set; }
    public string Vorname { get; set; } = string.Empty;
    public string Nachname { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
    public string FullName => string.Join(" ", new[] { Nachname?.Trim(), Vorname?.Trim() }.Where(s => !string.IsNullOrWhiteSpace(s))).Trim();
}
