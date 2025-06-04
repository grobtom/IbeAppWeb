namespace IbeAppWeb.DTOs.Umsatz;
public class AllProjectsUmsatzSplitDto
{
    public List<ProjektUmsatzDto> Projects { get; set; } = new();
    public decimal GesamtumsatzAllProjects => Projects.Sum(p => p.Gesamtumsatz);
}
