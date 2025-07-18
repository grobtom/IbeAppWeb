using IneAppWeb.DTOs;
using System.Text.Json.Serialization;

namespace IbeAppWeb.DTOs
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public DateTime? CreatedOnUtc { get; set; }
        public bool DbActive { get; set; }
        public bool IsSchacht { get; set; }
        public string? FileUrl { get; set; } = string.Empty;
        public int? BauleiterId { get; set; }
        public BauleiterDto? Bauleiter { get; set; }
        public string? BauleiterFullName { get; set; }
        public DateTime? DbChangedDate { get; set; }
        [JsonIgnore]
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public int? CustomerId { get; set; }
        public XCustomer? XCustomer { get; set; }
    }
}
