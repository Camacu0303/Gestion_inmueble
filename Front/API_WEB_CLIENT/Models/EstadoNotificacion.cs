﻿using System.ComponentModel.DataAnnotations;

namespace API_WEB_CLIENT.Models
{
    public class EstadoNotificacion
    {
        [Key]
        public int id_Estado { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
    }
}

