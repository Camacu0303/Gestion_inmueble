using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_WIN_MAIN.Models
{

    public class Contrato
    {
        [Key]
        public int id_contrato { get; set; }

        [ForeignKey("Propiedad")]
        public int id_propiedad { get; set; }

        [ForeignKey("Cliente")]
        public int id_cliente { get; set; }

        [Required]
        public DateTime fecha { get; set; }

        [Required]
        public decimal monto { get; set; }

        [Required]
        public int duracion { get; set; } // Meses

        [ForeignKey("EstadoContrato")]
        public int id_estado { get; set; }

        public Propiedad? Propiedad { get; set; }
        public Cliente? Cliente { get; set; }
        public EstadoContrato? EstadoContrato { get; set; }
    }

}
