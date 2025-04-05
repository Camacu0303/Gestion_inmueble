using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Models;

namespace WEB.Pages.Contratos
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;

        public IndexModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public List<Contrato>? Contratos { get; set; }
        public string? ApiError { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                var client = _httpClient.CreateClient("ApiClient");
                var response = await client.GetAsync("contratos?includes=propiedad,cliente,estado");

                if (response.IsSuccessStatusCode)
                {
                    Contratos = await response.Content.ReadFromJsonAsync<List<Contrato>>();
                }
                else
                {
                    ApiError = $"Error al cargar contratos: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                ApiError = $"Error de conexión: {ex.Message}";
            }
        }
    }
}