using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB.Models.PreProjectFiles
{
    public class Score
    {
        [Key]
        public int Id { get; set; }

        public int eaten { get; set; }

        public string duration { get; set; }

        [ForeignKey("Usuario")]
        public int UserId { get; set; }
        public Usuarios? Usuario { get; set; }
    }
}
