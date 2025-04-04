using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Models;

namespace WEB.Pages.Clientes
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;

        public DeleteModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Cliente Cliente { get; set; } = default!;

        public string? ApiError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            Cliente = await client.GetFromJsonAsync<Cliente>($"Clientes/{id}");

            if (Cliente == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            var response = await client.DeleteAsync($"Clientes/{id}");

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                Cliente = await client.GetFromJsonAsync<Cliente>($"Clientes/{id}");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}