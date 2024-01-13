using WebApplication2;

namespace WebApplication1.Models
{
    public class PassportViewModel
    {
        public int SotrudnikId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patranomic { get; set; }

        public List<Passport> PassporteList { get; set; }
    }
}
