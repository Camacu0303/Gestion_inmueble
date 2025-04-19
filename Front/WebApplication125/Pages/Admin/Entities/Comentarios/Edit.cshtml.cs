using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.Comentarios
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public EditModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Comentario Comentario { get; set; } = default!;

        public List<Propiedad>? Propiedades { get; set; } = default!;
        public List<Usuario>? Usuarios { get; set; } = default!;
        public List<Calificacion>? Calificaciones { get; set; } = default!;

        public string? ApiError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            Comentario = await EntityService.GetByIdAsync<Comentario>(client, id, "Comentario");
            if (Comentario == null)
            {
                return NotFound();
            }
            else {
                Propiedades = await EntityService.GetAllAsync<Propiedad>(client, "Propiedades");
                Usuarios = await EntityService.GetAllAsync<Usuario>(client, "Usuario");
                Calificaciones = await EntityService.GetAllAsync<Calificacion>(client, "Calificacion");
            } 
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");

            var existingEntity = await EntityService.GetByIdAsync<Comentario>(client, id, "Comentario");
            if (existingEntity == null)
                return NotFound();

            var response = await client.PutAsJsonAsync($"Comentario/{id}", Comentario);

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                Comentario = await client.GetFromJsonAsync<Comentario>($"Comentario/{id}");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
