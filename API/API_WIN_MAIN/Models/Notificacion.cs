using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_WIN_MAIN.Models
{
 
    public class Notificacion
    {
        [Key]
        public int id_Notificacion { get; set; }

        [ForeignKey("Usuario")]
        public int id_Usuario { get; set; }

        [Required]
        public string Mensaje { get; set; }

        [ForeignKey("EstadoNotificacion")]
        public int id_Estado { get; set; }

        public Usuario? Usuario { get; set; }
        public EstadoNotificacion? EstadoNotificacion { get; set; }
    }

}
