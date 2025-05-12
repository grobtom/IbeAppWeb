using System.ComponentModel;
using System.Text.Json.Serialization;

namespace IbeAppWeb.DTOs
{
    public class UmsatzDto
    {
        public string PkLvPos { get; set; } = string.Empty;
        public DateTime SaniertAm { get; set; }
        public string Fahrzeug { get; set; } = string.Empty;
        public string Kolonnenfuehrer { get; set; } = string.Empty;
        public decimal Umsatz { get; set; }
    }
}
