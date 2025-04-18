using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Agentes
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public EditModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Agente Agente { get; set; } = default!;
        
        public List<Usuario>? Usuarios { get; set; }
        public string? ApiError { get; set; }

        // Cargar los datos del Agente en el formulario
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            Agente = await EntityService.GetByIdAsync<Agente>(client, id, "Agente");

            if (Agente == null)
            {
                return NotFound();
            }
            else {
                Usuarios = await EntityService.GetAllAsync<Usuario>(client, "Usuario");
                Usuarios.ForEach(p => p.Contraseña = null);
            }

            return Page();
        }
        // Procesar el formulario para actualizar los datos del Agente
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");

            
            var existingEntity = await EntityService.GetByIdAsync<Agente>(client, id, "Agente");
            if (existingEntity == null)
            {
                return NotFound();
            }

            ValueHelper.PreserveOriginalValues(Agente, existingEntity);

            var response = await client.PutAsJsonAsync($"Agente/{id}", Agente);
            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                // Reload the agent data on failure to repopulate the form
                Agente = await client.GetFromJsonAsync<Agente>($"Agente/{id}");
                return Page();
            }

            // Redirect to the index page
            return RedirectToPage("./Index");
        }

    }
}
