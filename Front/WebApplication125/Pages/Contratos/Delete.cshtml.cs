using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WEB.Pages.Contratos
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;

        public DeleteModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Contrato Contrato { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            Contrato = await client.GetFromJsonAsync<Contrato>($"Contrato/{id}?includes=propiedad,cliente");

            if (Contrato == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClient.CreateClient("ApiClient");
            var response = await client.DeleteAsync($"Contrato/{Contrato.id_Contrato}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }

            TempData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return RedirectToPage("./Delete", new { id = Contrato.id_Contrato });
        }
    }
}
