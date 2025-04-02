using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_WIN_MAIN.Models
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
