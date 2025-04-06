namespace API_WIN_MAIN.DTOs.PropiedadesDTOs
{
    public class PropiedadDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public decimal Precio { get; set; }
        public string Tipo { get; set; }  // Nombre del tipo
        public string Estado { get; set; } // Nombre del estado
    }
}
