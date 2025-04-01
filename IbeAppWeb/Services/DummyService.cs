using IbeAppWeb.DTOs;

namespace IbeAppWeb.Services
{
    public class DummyService
    {
        public Task<List<UmsatzFlachDto>> GetUmsatzdatenAsync()
        {
            var data = new List<UmsatzFlachDto>
            {
                new() { Ausfuehrungsdatum = new DateTime(2025, 3, 25), Kolonnenfuehrer = "Meier", Typ = "MONTEUR", Mitarbeiter = "Meier", Gesamtumsatz = 500.123m },
                new() { Ausfuehrungsdatum = new DateTime(2025, 3, 25), Kolonnenfuehrer = "Meier", Typ = "MONTEUR", Mitarbeiter = "Schmidt", Gesamtumsatz = 500.123m },
                new() { Ausfuehrungsdatum = new DateTime(2025, 3, 25), Kolonnenfuehrer = "Meier", Typ = "FAHRZEUG", Mitarbeiter = "FZG-01", Gesamtumsatz = 500.123m },
                new() { Ausfuehrungsdatum = new DateTime(2025, 3, 26), Kolonnenfuehrer = "Müller", Typ = "MONTEUR", Mitarbeiter = "Müller", Gesamtumsatz = 300.000m },
                new() { Ausfuehrungsdatum = new DateTime(2025, 3, 26), Kolonnenfuehrer = "Müller", Typ = "FAHRZEUG", Mitarbeiter = "FZG-02", Gesamtumsatz = 300.000m },
            };

            return Task.FromResult(data);
        }
    }
}
