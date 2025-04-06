using System.Net.Http.Json;
using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
//using WEB.DTOs;

namespace WEB.Pages.Propiedades
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

        public List<PropiedadDto> Propiedades { get; set; } = new List<PropiedadDto>();

        public async Task OnGetAsync()
        {
            await CargarPropiedades();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var client = _httpClient.CreateClient("ApiClient");
                var response = await client.DeleteAsync($"Propiedades/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                    if (result?.success == true)
                    {
                        TempData["SuccessMessage"] = result.message;
                        _logger.LogInformation($"Propiedad {id} eliminada: {result.message}");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = result?.message ?? "La propiedad se eliminó pero no se recibió confirmación";
                        _logger.LogWarning($"Respuesta inesperada al eliminar propiedad {id}");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorResult = await response.Content.ReadFromJsonAsync<ApiResponse>();
                    TempData["ErrorMessage"] = errorResult?.message ?? "No se puede eliminar la propiedad";
                    _logger.LogWarning($"Error al eliminar propiedad {id}: {errorResult?.message}");
                }
                else
                {
                    TempData["ErrorMessage"] = $"Error: {response.StatusCode}";
                    _logger.LogError($"Error al eliminar propiedad {id}. Status: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error interno al procesar la solicitud";
                _logger.LogError(ex, $"Excepción al eliminar propiedad {id}");
            }

            await CargarPropiedades();
            return RedirectToPage();
        }

        private async Task CargarPropiedades()
        {
            try
            {
                var client = _httpClient.CreateClient("ApiClient");
                var response = await client.GetAsync("Propiedades");

                if (response.IsSuccessStatusCode)
                {
                    Propiedades = await response.Content.ReadFromJsonAsync<List<PropiedadDto>>() ?? new List<PropiedadDto>();
                }
                else
                {
                    _logger.LogError($"Error al cargar propiedades: {response.StatusCode}");
                    Propiedades = new List<PropiedadDto>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar propiedades");
                Propiedades = new List<PropiedadDto>();
            }
        }
    }

    public class PropiedadDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public decimal Precio { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
    }

    public class ApiResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
    }
}

