namespace IbeAppWeb.DTOs.Monteur;

public class UpdateMonteurDto
{
    public int MonteurId { get; set; }
    public string? Vorname { get; set; }
    public string? Nachname { get; set; }
    public string? Email { get; set; }
    public bool IsDeleted { get; set; } = false;
}
