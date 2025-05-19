using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbeAppWeb.DTOs.Umsatz;
public class AllProjectsUmsatzSplitDto
{
    public List<ProjektUmsatzDto> Projects { get; set; } = new();
    public decimal GesamtumsatzAllProjects => Projects.Sum(p => p.Gesamtumsatz);
}
