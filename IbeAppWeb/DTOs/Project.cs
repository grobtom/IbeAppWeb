namespace IbeAppWeb.DTOs
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public DateTime? CreatedOnUtc { get; set; }
        public bool DbActive { get; set; }
        public DateTime? DbChangedDate { get; set; }
        public bool IsSchacht { get; set; }
        public int? BauleiterId { get; set; }
        public string? BauleiterFullName { get; set; }
    }
}
