namespace IbeAppWeb.DTOs;
public class BauleiterWithProjectsDto : BauleiterDto
{
    public List<Project> Projects { get; set; } = new List<Project>();
}
