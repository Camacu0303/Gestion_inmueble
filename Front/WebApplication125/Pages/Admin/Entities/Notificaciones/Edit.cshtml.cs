using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.Notificaciones
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public EditModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Notificacion Notificacion { get; set; } = default!;
        public List<Usuario>? Usuarios { get; set; } = default!;
        public List<EstadoNotificacion>? EstadosNotificacion { get; set; } = default!;

        public string? ApiError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            Notificacion = await EntityService.GetByIdAsync<Notificacion>(client, id, "Notificacion");
            if (Notificacion == null)
            {
                return NotFound();
            }
            else {
               
                Usuarios = await EntityService.GetAllAsync<Usuario>(client, "Usuario");
                EstadosNotificacion = await EntityService.GetAllAsync<EstadoNotificacion>(client, "EstadoNotificacion");
            } 
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");

            var existingEntity = await EntityService.GetByIdAsync<Notificacion>(client, id, "Notificacion");
            if (existingEntity == null)
                return NotFound();

            var response = await client.PutAsJsonAsync($"Notificacion/{id}", Notificacion);

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                Notificacion = await client.GetFromJsonAsync<Notificacion>($"Notificacion/{id}");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
