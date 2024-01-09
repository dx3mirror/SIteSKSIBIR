using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class SotrudnikInputModel
    {
        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Firstname { get; set; }

        public string Patranomic { get; set; }

        public string Adress { get; set; }

        public string MestoRojd { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Datarojdeniy { get; set; }

        public string IdentityNomer { get; set; }

        public string Okin { get; set; }

        // Другие поля, если необходимо
    }
}
