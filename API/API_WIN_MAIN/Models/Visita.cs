using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_WIN_MAIN.Models
{
 
    public class Visita
    {
        [Key]
        public int id_Visita { get; set; }

        [ForeignKey("Cliente")]
        public int id_Cliente { get; set; }

        [ForeignKey("Propiedad")]
        public int id_Propiedad { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [ForeignKey("Agente")]
        public int id_Agente { get; set; }

        public Cliente? Cliente { get; set; }
        public Propiedad? Propiedad { get; set; }
        public Agente? Agente { get; set; }
    }

}
