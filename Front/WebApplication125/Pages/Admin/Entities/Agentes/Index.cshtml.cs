using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Models;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.Agentes
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;

        public IndexModel(IHttpClientFactory httpClient, ILogger<IndexModel> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public List<Agente> Agentes { get; set; } = new List<Agente>();

        public async Task OnGetAsync()
        {
            try
            {
                var client = _httpClient.CreateClient("ApiClient");
                Agentes = await EntityService.GetAllAsync<Agente>(client, "Agente");

                Agentes = Agentes.Select(a => new Agente
                {
                    id_Agente = a.id_Agente,
                    id_Usuario = a.id_Usuario,
                    Telefono = a.Telefono,
                    Experiencia = a.Experiencia,
                    Usuario= a.Usuario
                }).ToList();


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar agentes");
            }
        }



        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var client = _httpClient.CreateClient("ApiClient");
                var response = await client.DeleteAsync($"Agente/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Agente eliminado correctamente";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al eliminar el contrato";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar contrato");
                TempData["ErrorMessage"] = "Error interno al eliminar el contrato";
            }

            return RedirectToPage();
        }
    }
}
