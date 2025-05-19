using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbeAppWeb.DTOs.Umsatz;
public class FahrzeugUmsatzDto
{
    public string ProjectName { get; set; }
    public string FieldName { get; set; }
    public string Fahrzeug { get; set; }
    public decimal Gesamtumsatz { get; set; }
}
