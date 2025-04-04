using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Models;

namespace WEB.Pages.Propiedades
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;

        public DeleteModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Propiedad Propiedad { get; set; } = default!;

        public string? ApiError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            Propiedad = await client.GetFromJsonAsync<Propiedad>($"Propiedad/{id}");

            if (Propiedad == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            var response = await client.DeleteAsync($"Propiedad/{id}");

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                Propiedad = await client.GetFromJsonAsync<Propiedad>($"Propiedad/{id}");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}