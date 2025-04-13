using System.ComponentModel.DataAnnotations;

namespace WEB.Models.Credentials
{
    public class NewUser
    {
        [Required, StringLength(100)]
        public string nombre { get; set; }

        [Required, StringLength(100)]
        public string email { get; set; }

        [Required, StringLength(255)]
        public string contraseña { get; set; }
    }
}
