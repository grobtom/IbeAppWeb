namespace IbeAppWeb.DTOs
{
    public class ArbeitsscheinResultDto
    {
        public List<ArbeitsscheinDto>? Arbeitsscheine { get; set; } = new();
        public List<LinkDto>? Links { get; set; } = new();
    }
}
