using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Models;

namespace WEB.Pages.Contratos
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IHttpClientFactory httpClient, ILogger<IndexModel> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public List<ContratoDto> Contratos { get; set; } = new List<ContratoDto>();

        public async Task OnGetAsync()
        {
            try
            {
                var client = _httpClient.CreateClient("ApiClient");
                Contratos = await client.GetFromJsonAsync<List<ContratoDto>>("Contratos") ?? new List<ContratoDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar contratos");
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var client = _httpClient.CreateClient("ApiClient");
                var response = await client.DeleteAsync($"Contratos/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Contrato eliminado correctamente";
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
    public class ContratoDto
    {
        public int Id { get; set; }
        public string PropiedadNombre { get; set; }
        public string ClienteNombre { get; set; }
        public string Tipo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal Monto { get; set; }
        public string EstadoNombre { get; set; }
    }
}