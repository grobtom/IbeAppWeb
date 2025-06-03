using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbeAppWeb.DTOs.Umsatz;
public class ProjektUmsatzDto
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; }
    public bool IsSchacht { get; set; }
    public decimal Gesamtumsatz { get; set; }
    public decimal GesamtumsatzMitDatum { get; set; }
    public decimal GesamtumsatzAlle { get; set; } = 0;

    public List<MonteurUmsatzDto> MonteurUmsaetze { get; set; } = new();
    public List<FahrzeugUmsatzDto> FahrzeugUmsaetze { get; set; } = new();
}
