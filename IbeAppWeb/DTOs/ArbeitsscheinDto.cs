﻿using IbeAppWeb.DTOs.Monteur;

namespace IbeAppWeb.DTOs;

public class ArbeitsscheinDto
{
    public string? ProjectName { get; set; }
    public string? HALTUNG { get; set; }
    public string? LVPosition { get; set; }
    public string? Firma { get; set; }
    public string? Kolonnenfuehrer { get; set; }
    public string? Monteur { get; set; }
    public List<MonteurDto> MonteurList { get; set; } = new();
    public string? AnlageName { get; set; }
    public List<FahrzeugDto> FahrzeugList { get; set; } = new();
    public decimal Menge { get; set; }
    public DateTime SaniertAm { get; set; }
    public string? Abschlagsrechnung { get; set; }
    public string? ArbBerNr { get; set; }
    public string? BauBerNr { get; set; }
    public string? StdBerNr { get; set; }
    public string? Kurztext { get; set; }
    public decimal Preis { get; set; }
    public string? Einheit { get; set; }
    public decimal Positionssumme { get; set; }
}


