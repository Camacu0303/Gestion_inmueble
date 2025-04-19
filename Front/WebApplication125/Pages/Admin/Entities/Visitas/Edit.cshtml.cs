using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.Visitas
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public EditModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Visita Visita { get; set; } = default!;

        [BindProperty]
        public String fecha { get; set; }
        public List<Propiedad>? Propiedades { get; set; }
        public List<Cliente>? Clientes { get; set; }
        public List<Agente>? Agentes { get; set; }

        public string? ApiError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            Visita = await EntityService.GetByIdAsync<Visita>(client, id, "Visita");

            if (Visita == null)
                return NotFound();

            // Formatear fecha al estilo yyyy-MM-dd
            fecha = Visita.Fecha.ToString("yyyy-MM-dd");

            Propiedades = await EntityService.GetAllAsync<Propiedad>(client, "Propiedades");
            Clientes = await EntityService.GetAllAsync<Cliente>(client, "Cliente");
            Agentes = await EntityService.GetAllAsync<Agente>(client, "Agente");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");

            var existingEntity = await EntityService.GetByIdAsync<Visita>(client, id, "Visita");
            if (existingEntity == null)
                return NotFound();

            if (DateTime.TryParse(fecha, out var fechaParsed))
            {
                Visita.Fecha = fechaParsed;
            }

            var response = await client.PutAsJsonAsync($"Visita/{id}", Visita);

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                Visita = await client.GetFromJsonAsync<Visita>($"Visita/{id}");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
