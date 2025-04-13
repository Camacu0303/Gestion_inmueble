using System.ComponentModel.DataAnnotations;

namespace WEB.Models.Credentials
{
    public class LoginUser
    {
        [Required, StringLength(100)]
        public string email { get; set; }

        [Required, StringLength(100)]
        public string contraseña { get; set; }
    }
}
