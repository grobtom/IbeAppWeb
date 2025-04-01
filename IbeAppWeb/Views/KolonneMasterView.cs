using IbeAppWeb.DTOs;

namespace IbeAppWeb.Views
{
    public class KolonneMasterView
    {
        public DateTime Ausfuehrungsdatum { get; set; }
        public string Kolonnenfuehrer { get; set; }
        public List<UmsatzFlachDto> Details { get; set; } = new();
    }
}
