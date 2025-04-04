using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WEB.Pages.Clientes
{
    public class EditModel : PageModel
    {
            private readonly IHttpClientFactory _httpClient;

            public EditModel(IHttpClientFactory httpClient)
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

            public async Task<IActionResult> OnPostAsync()
            {
                if (!ModelState.IsValid)
                    return Page();

                var client = _httpClient.CreateClient("ApiClient");
                var response = await client.PutAsJsonAsync($"Clientes/{Cliente.id_Cliente}", Cliente);

                if (!response.IsSuccessStatusCode)
                {
                    ApiError = await response.Content.ReadAsStringAsync();
                    return Page();
                }

                return RedirectToPage("./Index");
            }
        }
    }

