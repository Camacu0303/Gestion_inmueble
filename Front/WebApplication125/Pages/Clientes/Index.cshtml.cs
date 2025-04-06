using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WEB.Pages.Clientes
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;

        public IndexModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public List<Cliente>? Clientes { get; set; }
        public string? ApiError { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                var client = _httpClient.CreateClient("ApiClient");
                var response = await client.GetAsync("Cliente");

                if (response.IsSuccessStatusCode)
                {
                    Clientes = await response.Content.ReadFromJsonAsync<List<Cliente>>();
                }
                else
                {
                    ApiError = $"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                }
            }
            catch (Exception ex)
            {
                ApiError = $"Error al conectar con la API: {ex.Message}";
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            var response = await client.DeleteAsync($"clientes/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Cliente eliminado correctamente";
            }
            else
            {
                TempData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            }

            return RedirectToPage();
        }
    }
}
