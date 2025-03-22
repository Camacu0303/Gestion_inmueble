
using System.ComponentModel.DataAnnotations;
namespace API_WIN_MAIN.Models
{
 
    public class Calificacion
    {
        [Key]
        public int id_Calificacion { get; set; }

        [Required]
        [Range(1, 5)]
        public int Valor { get; set; }
    }

}
