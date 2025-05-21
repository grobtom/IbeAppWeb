using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IbeAppWeb.DTOs;
public class BauleiterWithProjectsDto : BauleiterDto
{
    public List<Project> Projects { get; set; } = new List<Project>();
}
