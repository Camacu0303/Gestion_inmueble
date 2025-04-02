using API_WIN_MAIN.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace API_WIN_MAIN.Models
{
    public class Usuario
    {
        [Key]
        public int id_Usuario { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Contraseña { get; set; }

    }
}



