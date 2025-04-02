
using System.ComponentModel.DataAnnotations;
namespace API_WIN_MAIN.Models
{


    public class TipoPropiedad
    {
        [Key]
        public int id_Tipo { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
    }

}
