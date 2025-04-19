using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.EstadosPago
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public EditModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public EstadoPago EstadoPago { get; set; } = default!;


        public string? ApiError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            EstadoPago = await EntityService.GetByIdAsync<EstadoPago>(client, id, "EstadoPago");
            if (EstadoPago == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");

            var existingEntity = await EntityService.GetByIdAsync<EstadoPago>(client, id, "EstadoPago");
            if (existingEntity == null)
                return NotFound();

            var response = await client.PutAsJsonAsync($"EstadoPago/{id}", EstadoPago);

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                EstadoPago = await client.GetFromJsonAsync<EstadoPago>($"EstadoPago/{id}");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
