using System.ComponentModel.DataAnnotations;
namespace API_WIN_MAIN.Models
{
    public class Rol
    {
        [Key]
        public int id_Rol { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
    }

}
