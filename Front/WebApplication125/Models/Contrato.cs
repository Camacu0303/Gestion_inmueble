using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_WIN_MAIN.Models
{

    public class Contrato
    {
        [Key]
        public int id_Contrato { get; set; }

        [ForeignKey("Propiedad")]
        public int id_Propiedad { get; set; }

        [ForeignKey("Cliente")]
        public int id_Cliente { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public decimal Monto { get; set; }

        [Required]
        public int Duracion { get; set; } // Meses

        [ForeignKey("EstadoContrato")]
        public int id_Estado { get; set; }

        public Propiedad Propiedad { get; set; }
        public Cliente Cliente { get; set; }
        public EstadoContrato EstadoContrato { get; set; }
    }

}
