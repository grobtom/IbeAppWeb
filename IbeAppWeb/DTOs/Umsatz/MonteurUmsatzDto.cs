namespace IbeAppWeb.DTOs.Umsatz;
public class MonteurUmsatzDto
{
    public string ProjectName { get; set; } = string.Empty;
    public string FieldName { get; set; } = string.Empty;
    public string Monteur { get; set; } = string.Empty;
    public decimal Gesamtumsatz { get; set; }
}
