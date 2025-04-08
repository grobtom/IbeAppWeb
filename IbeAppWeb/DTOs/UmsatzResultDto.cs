namespace IbeAppWeb.DTOs
{
    public class UmsatzResultDto
    {
        public List<UmsatzDto> UmsatzFahrzeugDatum { get; set; } = new();
        public List<LinkDto> Links { get; set; } = new();
    }
}
