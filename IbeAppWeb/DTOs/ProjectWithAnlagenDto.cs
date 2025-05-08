namespace IbeAppWeb.DTOs;


    public class ProjectWithAnlagenDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<AnlageDto> Anlagen { get; set; } = new();
    }
