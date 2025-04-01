namespace IbeAppWeb.DTOs
{
    public class UmsatzFlachDto
    {
        public DateTime Ausfuehrungsdatum { get; set; }
        public string Kolonnenfuehrer { get; set; }
        public string Typ { get; set; }
        public string Mitarbeiter { get; set; }
        public decimal Gesamtumsatz { get; set; }
    }
}
