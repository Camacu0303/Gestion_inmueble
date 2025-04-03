using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication125.Services;
using API_WIN_MAIN.Models;

namespace WEB.Pages.Contracts
{
    public class AddContractStateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public AddContractStateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public List<EstadoContrato> EstadoContratos { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                EstadoContratos = await _httpClient.GetFromJsonAsync<List<EstadoContrato>>("EstadoContrato") ?? new List<EstadoContrato>();
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Error de conexión: {ex.Message}");
            }
        }
        [BindProperty]
        public EstadoContrato NuevoEstadoContrato { get; set; } = new EstadoContrato();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var response = await _httpClient.PostAsJsonAsync("EstadoContrato", NuevoEstadoContrato);

                if (response.IsSuccessStatusCode)
                    return RedirectToPage("Index");

                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Error de la API: {errorMessage}");
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Error de conexión: {ex.Message}");
            }

            return Page();
        }



    }
}