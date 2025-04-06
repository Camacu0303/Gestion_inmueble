namespace API_WIN_MAIN.DTOs.ContratosDTOs
{
    public class ContratoDto
    {
        public int Id { get; set; }
        public string PropiedadNombre { get; set; }
        public string ClienteNombre { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public int Duracion { get; set; }
        public string Estado { get; set; }
    }
}
