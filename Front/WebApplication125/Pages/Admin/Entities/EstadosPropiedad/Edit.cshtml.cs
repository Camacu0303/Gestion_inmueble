using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.EstadosPropiedad
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public EditModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public EstadoPropiedad EstadoPropiedad { get; set; } = default!;


        public string? ApiError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            EstadoPropiedad = await EntityService.GetByIdAsync<EstadoPropiedad>(client, id, "EstadoPropiedad");
            if (EstadoPropiedad == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");

            var existingEntity = await EntityService.GetByIdAsync<EstadoPropiedad>(client, id, "EstadoPropiedad");
            if (existingEntity == null)
                return NotFound();

            var response = await client.PutAsJsonAsync($"EstadoPropiedad/{id}", EstadoPropiedad);

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                EstadoPropiedad = await client.GetFromJsonAsync<EstadoPropiedad>($"EstadoPropiedad/{id}");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
