using System.ComponentModel.DataAnnotations;

namespace API_WEB_CLIENT.Models
{
    public class Cliente
    {
        [Key]
        public int id_Cliente { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [Required]
        public decimal Presupuesto { get; set; }
    }
}
