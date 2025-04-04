using System.Net.Http.Json;
using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WEB.Pages.Propiedades
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;

        public IndexModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public List<Propiedad>? Propiedades { get; set; }

        public async Task OnGetAsync()
        {
            var client = _httpClient.CreateClient("ApiClient");
            Propiedades = await client.GetFromJsonAsync<List<Propiedad>>("Propiedad"); // Endpoint: GET /api/Propiedad
        }
    }
}