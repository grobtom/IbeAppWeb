using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbeAppWeb.DTOs.Umsatz;
public class FahrzeugUmsatzDto
{
    public string ProjectName { get; set; } = string.Empty;
    public string FieldName { get; set; } = string.Empty;
    public string Fahrzeug { get; set; } = string.Empty;
    public decimal Gesamtumsatz { get; set; }
}
