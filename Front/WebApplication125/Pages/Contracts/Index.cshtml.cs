using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication125.Services;
using API_WIN_MAIN.Models;

namespace WebApplication125.Pages.Contracts { 
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory httpClientFactory)
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



    }
}