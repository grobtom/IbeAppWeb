namespace IbeAppWeb.DTOs
{
    public class ProjectAnlageDto
    {
        public int ProjectId { get; set; }
        public int AnlageId { get; set; }
        public string? AnlageName { get; set; } = string.Empty;
        public string? ProjectName { get; set; } = string.Empty;
    }
}
