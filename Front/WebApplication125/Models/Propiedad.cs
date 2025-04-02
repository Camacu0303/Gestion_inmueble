using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_WIN_MAIN.Models
{
    public class Propiedad
    {
        [Key]
        public int id_Propiedad { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(255)]
        public string Direccion { get; set; }

        [Required]
        public decimal Precio { get; set; }

        [ForeignKey("TipoPropiedad")]
        public int id_Tipo { get; set; }

        [ForeignKey("EstadoPropiedad")]
        public int id_Estado { get; set; }

        [ForeignKey("Usuario")]
        public int id_Usuario { get; set; }

        public TipoPropiedad TipoPropiedad { get; set; }
        public EstadoPropiedad EstadoPropiedad { get; set; }
        public Usuario Usuario { get; set; }
    }

}
