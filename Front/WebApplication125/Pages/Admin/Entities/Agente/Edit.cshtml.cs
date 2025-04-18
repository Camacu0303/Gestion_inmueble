using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.DTOs.AgenteDto;
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

        public string? ApiError { get; set; }

        // Cargar los datos del Agente en el formulario
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            Agente = await client.GetFromJsonAsync<Agente>($"Agente/{id}");

            if (Agente == null)
                return NotFound();

            return Page();
        }
        // Procesar el formulario para actualizar los datos del Agente
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");

            // Load the existing entity from the database (same way as OnGetAsync)
            var existingEntity = await client.GetFromJsonAsync<Agente>($"Agente/{id}");
            if (existingEntity == null)
            {
                return NotFound();
            }

            // Preserve original values for properties not on the form
            ValueHelper.PreserveOriginalValues(Agente, existingEntity);

            // If the user didn't input a new password (the input field for password is absent),
            // we ensure the original password is sent back to the API.
            if (Agente.Usuario != null && string.IsNullOrEmpty(Agente.Usuario.Contraseña) && existingEntity.Usuario != null)
            {
                Agente.Usuario.Contraseña = existingEntity.Usuario.Contraseña;
            }

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
