using System.ComponentModel.DataAnnotations;

namespace API_WIN_MAIN.DTOs.PropiedadesDTOs
{
    public class PropiedadUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La dirección es requerida")]
        [StringLength(255, ErrorMessage = "Máximo 255 caracteres")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El tipo es requerido")]
        public int IdTipo { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        public int IdEstado { get; set; }
    }
}
