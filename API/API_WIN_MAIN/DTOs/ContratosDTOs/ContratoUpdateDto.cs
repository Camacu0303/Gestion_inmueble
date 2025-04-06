using System.ComponentModel.DataAnnotations;

namespace API_WIN_MAIN.DTOs.ContratosDTOs
{
    public class ContratoUpdateDto
    {
        public int Id { get; set; }

        [Required]
        public int IdPropiedad { get; set; }

        [Required]
        public int IdCliente { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public decimal Monto { get; set; }

        [Required]
        public int Duracion { get; set; }

        [Required]
        public int IdEstado { get; set; }
    }
}
