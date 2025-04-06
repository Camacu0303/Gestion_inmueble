namespace API_WIN_MAIN.DTOs.PropiedadesDTOs
{
    public class PropiedadDetailDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public decimal Precio { get; set; }
        public int IdTipo { get; set; }
        public int IdEstado { get; set; }
        public int IdUsuario { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
    }
}
