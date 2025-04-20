using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using WEB.Util;

namespace WebApplication125.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;

        public IndexModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Propiedad Propiedad { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImagenFile { get; set; }

        public List<EstadoPropiedad>? EstadosPropiedad { get; set; }
        public List<TipoPropiedad>? TiposPropiedad { get; set; }
        public List<Usuario>? Usuarios { get; set; }
        public List<Propiedad>? Propiedades { get; set; }
        public string? ApiError { get; set; }


        public async Task OnGetAsync()
        {
            var client = _httpClient.CreateClient("ApiClient");
            Propiedades = await EntityService.GetAllAsync<Propiedad>(client, "Propiedades");                

            EstadosPropiedad = await EntityService.GetAllAsync<EstadoPropiedad>(client, "EstadoPropiedad");
            TiposPropiedad = await EntityService.GetAllAsync<TipoPropiedad>(client, "TipoPropiedad");
            Usuarios = await EntityService.GetAllAsync<Usuario>(client, "Usuario");
            Propiedades = await EntityService.GetAllAsync<Propiedad>(client, "Propiedades");

            if (Propiedades == null || !Propiedades.Any())
                ApiError = "No se encontraron propiedades.";

            
        }



    }
}
