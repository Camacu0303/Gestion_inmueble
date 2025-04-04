using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WEB.Pages.Clientes
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;

        public CreateModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Cliente Cliente { get; set; } = new Cliente();

        public string? ApiError { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var client = _httpClient.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("Cliente", Cliente);

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
