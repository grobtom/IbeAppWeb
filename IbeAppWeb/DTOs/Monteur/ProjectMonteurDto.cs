using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbeAppWeb.DTOs.Monteur;

/// <summary>
/// Represents a data transfer object for a project monteur, containing details about the project and its associated
/// monteur.
/// </summary>

public class ProjectMonteurDto
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public bool DbActive { get; set; }
    public DateTime? CreatedOnUtc { get; set; }
    public bool IsSchacht { get; set; }
    public string? FileUrl { get; set; } = string.Empty;
    public int? BauleiterId { get; set; }
    public string? BauleiterFullName { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime LastModifiedAt { get; set; }

    /// <summary>
    /// The ID of the monteur associated with this project.
    /// </summary>
    public int MonteurId { get; set; }

}
