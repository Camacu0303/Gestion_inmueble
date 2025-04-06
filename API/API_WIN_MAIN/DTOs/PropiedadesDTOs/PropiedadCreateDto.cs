using System.ComponentModel.DataAnnotations;

namespace API_WIN_MAIN.DTOs.PropiedadesDTOs
{
    public class PropiedadCreateDto
    {
        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [Required, StringLength(255)]
        public string Direccion { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public decimal Precio { get; set; }

        [Required]
        public int id_Tipo { get; set; }

        [Required]
        public int id_Estado { get; set; }

        [Required]
        public int id_Usuario { get; set; }
    }
}
