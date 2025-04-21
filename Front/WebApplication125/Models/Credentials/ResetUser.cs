using System.ComponentModel.DataAnnotations;

namespace API_WIN_MAIN.DTOs.Credentials
{
    public class ResetUser
    {
        [Required, StringLength(100)]
        public string email { get; set; }

        [Required, StringLength(255)]
        public string contraseña { get; set; }
        [Required]
        public String Token { get; set; }
    }
}
