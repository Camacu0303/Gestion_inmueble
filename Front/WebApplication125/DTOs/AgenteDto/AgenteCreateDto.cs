using System.ComponentModel.DataAnnotations;

namespace WEB.DTOs.AgenteDto
{
    public class AgenteCreateDto
    {
        [Required(ErrorMessage = "El ID de usuario es requerido")]
        public int id_Usuario { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "La experiencia es requerida")]
        [Range(0, 100, ErrorMessage = "La experiencia debe estar entre 0 y 100 años")]
        public int Experiencia { get; set; }
    }
}
