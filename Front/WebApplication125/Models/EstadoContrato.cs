
using System.ComponentModel.DataAnnotations;
namespace API_WIN_MAIN.Models
{

    public class EstadoContrato
    {
        [Key]
        public int id_Estado { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
    }

}
