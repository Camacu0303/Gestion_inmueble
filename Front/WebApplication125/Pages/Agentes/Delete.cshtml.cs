using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WEB.Pages.Agentes
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;

        public DeleteModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Agente Agente { get; set; } = default!;

        public string? ApiError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            Agente = await client.GetFromJsonAsync<Agente>($"Agente/{id}");

            if (Agente == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            var response = await client.DeleteAsync($"Agente/{id}");

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                Agente = await client.GetFromJsonAsync<Agente>($"Agente/{id}");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
