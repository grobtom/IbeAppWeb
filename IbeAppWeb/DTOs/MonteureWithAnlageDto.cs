namespace IbeAppWeb.DTOs;

public class MonteurWithAnlageDto
{
    public int MonteurId { get; set; }
    public string? Vorname { get; set; }
    public string? Nachname { get; set; }
    public int? AnlageId { get; set; }
    public string? AnlageName { get; set; }
}
