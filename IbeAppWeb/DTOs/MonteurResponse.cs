namespace IbeAppWeb.DTOs;

public class MonteurResponse
{
    public int MonteurId { get; set; }
    public string Vorname { get; set; }
    public string Nachname { get; set; }
    public bool IsDeleted { get; set; }
    public int? Anlageid { get; set; } = 0;
}