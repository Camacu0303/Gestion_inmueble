using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API_WIN_MAIN.Models
{

    public class Pago
    {
        [Key]
        public int id_Pago { get; set; }

        [ForeignKey("Cliente")]
        public int id_Cliente { get; set; }

        [ForeignKey("Propiedad")]
        public int id_Propiedad { get; set; }

        [Required]
        public decimal Monto { get; set; }

        [ForeignKey("EstadoPago")]
        public int id_Estado { get; set; }

        public Cliente? Cliente { get; set; }
        public Propiedad? Propiedad { get; set; }
        public EstadoPago? EstadoPago { get; set; }
    }

}
