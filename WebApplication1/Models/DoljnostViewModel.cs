using WebApplication2;

namespace WebApplication1.Models
{
    public class DoljnostViewModel
    {
        public int SotrudnikId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patranomic { get; set; }

        public List<Doljnost> DoljnostList { get; set; }
    }
}
