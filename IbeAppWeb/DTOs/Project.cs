namespace IbeAppWeb.DTOs
{
    public class Project
    {
        public int ID { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public DateTime? CreatedOnUtc { get; set; }
        public bool DbActive { get; set; }
        public DateTime? DbChangedDate { get; set; }
    }
}
