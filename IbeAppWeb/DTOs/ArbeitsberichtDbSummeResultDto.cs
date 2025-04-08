namespace IbeAppWeb.DTOs;

public class ArbeitsberichtDbSummeResultDto
{
    public List<ArbeitsberichtDbSummeDto> Arbeitsberichte { get; set; } = new();
    public List<LinkDto> Links { get; set; } = new();
}
