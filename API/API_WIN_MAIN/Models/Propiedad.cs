using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_WIN_MAIN.Models
{
    public class Propiedad
    {
        [Key]
        public int id_propiedad { get; set; }

        [Required, StringLength(100)]
        public string nombre { get; set; }

        [Required, StringLength(255)]
        public string direccion { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal precio { get; set; }

        [ForeignKey("TipoPropiedad")]
        public int id_tipo { get; set; }

        [ForeignKey("EstadoPropiedad")]
        public int id_estado { get; set; }

        [ForeignKey("Usuario")]
        public int id_usuario { get; set; }

        public String? detalles { get; set; }

        public virtual TipoPropiedad? TipoPropiedad { get; set; }
        public virtual EstadoPropiedad? EstadoPropiedad { get; set; }
        public virtual Usuario? Usuario { get; set; }
    }

}
