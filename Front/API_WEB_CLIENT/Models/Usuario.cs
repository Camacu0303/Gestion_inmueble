﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_WEB_CLIENT.Models
{
    public class Usuario
    {
        [Key]
        public int id_Usuario { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Contraseña { get; set; }

        [ForeignKey("Rol")]
        public int id_Rol { get; set; }

        public Rol Rol { get; set; }
    }
}
