using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.EstadosNotificacion
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public EditModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public EstadoNotificacion EstadoNotificacion { get; set; } = default!;


        public string? ApiError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            EstadoNotificacion = await EntityService.GetByIdAsync<EstadoNotificacion>(client, id, "EstadoNotificacion");
            if (EstadoNotificacion == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");

            var existingEntity = await EntityService.GetByIdAsync<EstadoNotificacion>(client, id, "EstadoNotificacion");
            if (existingEntity == null)
                return NotFound();

            var response = await client.PutAsJsonAsync($"EstadoNotificacion/{id}", EstadoNotificacion);

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                EstadoNotificacion = await client.GetFromJsonAsync<EstadoNotificacion>($"EstadoNotificacion/{id}");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
