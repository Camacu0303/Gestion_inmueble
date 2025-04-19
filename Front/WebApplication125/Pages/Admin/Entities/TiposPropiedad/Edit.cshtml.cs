using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.TiposPropiedad
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public EditModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public TipoPropiedad TipoPropiedad { get; set; } = default!;


        public string? ApiError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            TipoPropiedad = await EntityService.GetByIdAsync<TipoPropiedad>(client, id, "TipoPropiedad");
            if (TipoPropiedad == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");

            var response = await client.PutAsJsonAsync($"TipoPropiedad/{id}", TipoPropiedad);

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                TipoPropiedad = await client.GetFromJsonAsync<TipoPropiedad>($"TipoPropiedad/{id}");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
