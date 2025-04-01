using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication125.Models
{
    public class Score
    {
        [Key]
        public int Id { get; set; }

        public int eaten { get; set; }

        public String duration { get; set; }

        [ForeignKey("Usuario")]
        public int UserId { get; set; }
        public Usuarios? Usuario { get; set; }
    }
}
