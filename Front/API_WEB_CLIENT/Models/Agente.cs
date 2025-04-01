using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_WEB_CLIENT.Models
{
    public class Agente
    {
        [Key]
        public int id_Agente { get; set; }

        [ForeignKey("Usuario")]
        public int id_Usuario { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [Required]
        public int Experiencia { get; set; } // Años de experiencia

        public Usuario Usuario { get; set; }
    }
}
