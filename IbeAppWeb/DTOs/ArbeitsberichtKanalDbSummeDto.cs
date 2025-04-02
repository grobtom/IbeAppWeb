﻿using System.ComponentModel;
using System.Text.Json.Serialization;

namespace IbeAppWeb.DTOs
{
    public class ArbeitsberichtKanalDbSummeDto
    {
        public string? Projektname { get; set; }
        public List<ArbeitsscheinDto> Arbeitsscheine { get; set; } = new();
        public decimal Summe { get; set; }
    }
}
