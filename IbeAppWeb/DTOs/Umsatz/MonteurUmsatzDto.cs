using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbeAppWeb.DTOs.Umsatz;
public class MonteurUmsatzDto
{
    public string ProjectName { get; set; }
    public string FieldName { get; set; }
    public string Monteur { get; set; }
    public decimal Gesamtumsatz { get; set; }
}
