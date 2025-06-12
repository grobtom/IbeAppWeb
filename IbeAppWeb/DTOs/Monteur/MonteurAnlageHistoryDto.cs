using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbeAppWeb.DTOs.Monteur;

/// <summary>
/// Represents the history of assignments between a technician (Monteur) and a facility (Anlage).
/// </summary>
/// <remarks>This data transfer object (DTO) is used to track changes in the relationship between a technician and
/// a facility,  including the assignment date, associated names, and any relevant comments or actions.</remarks>

public class MonteurAnlageHistoryDto
{
    public int Id { get; set; }
    public int MonteurId { get; set; }
    public string? MonteurName { get; set; } = string.Empty;
    public int? AnlageId { get; set; }
    public string? AnlageName { get; set; } = string.Empty;
    public DateTime WechselDatum { get; set; }
    public string? Kommentar { get; set; } = string.Empty;
    public string? Action { get; set; } = string.Empty; 
}
