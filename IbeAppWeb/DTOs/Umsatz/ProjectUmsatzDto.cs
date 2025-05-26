using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbeAppWeb.DTOs.Umsatz;
public class ProjektUmsatzDto
{
    public string ProjectName { get; set; } = string.Empty;
    public decimal Gesamtumsatz { get; set; }

    public List<MonteurUmsatzDto> MonteurUmsaetze { get; set; } = new();
    public List<FahrzeugUmsatzDto> FahrzeugUmsaetze { get; set; } = new();
}
