using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.EstadosContrato
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public EditModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public EstadoContrato EstadoContrato { get; set; } = default!;


        public string? ApiError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            EstadoContrato = await EntityService.GetByIdAsync<EstadoContrato>(client, id, "EstadoContrato");
            if (EstadoContrato == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");

            var existingEntity = await EntityService.GetByIdAsync<EstadoContrato>(client, id, "EstadoContrato");
            if (existingEntity == null)
                return NotFound();

            var response = await client.PutAsJsonAsync($"EstadoContrato/{id}", EstadoContrato);

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                EstadoContrato = await client.GetFromJsonAsync<EstadoContrato>($"EstadoContrato/{id}");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
