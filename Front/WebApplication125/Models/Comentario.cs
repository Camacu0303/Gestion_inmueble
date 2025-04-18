using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_WIN_MAIN.Models
{

    public class Comentario
    {
        [Key]
        public int id_Comentario { get; set; }

        [ForeignKey("Usuario")]
        public int id_Usuario { get; set; }

        [ForeignKey("Propiedad")]
        public int id_Propiedad { get; set; }

        [Required]
        public string Mensaje { get; set; }

        [ForeignKey("Calificacion")]
        public int id_Calificacion { get; set; }

        public Usuario? Usuario { get; set; }
        public Propiedad? Propiedad { get; set; }
        public Calificacion? Calificacion { get; set; }
    }

}
