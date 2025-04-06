namespace API_WIN_MAIN.DTOs.ContratosDTOs
{
    public class ContratoDetailDto
    {
        public int Id { get; set; }
        public int IdPropiedad { get; set; }
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public int Duracion { get; set; }
        public int IdEstado { get; set; }

        public string PropiedadNombre { get; set; }
        public string ClienteNombre { get; set; }
        public string EstadoNombre { get; set; }
    }
    }

