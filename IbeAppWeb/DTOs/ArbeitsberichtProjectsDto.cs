namespace IbeAppWeb.DTOs;

public class ArbeitsberichtProjectsDto
{
    public List<ArbeitsberichtDbSummeDto> ArbeitsberichtProjekt { get; init; } = new();
    public List<LinkDto> Links { get; init; } = new();
}
