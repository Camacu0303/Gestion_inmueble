using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.Contratos
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public EditModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Cliente Cliente { get; set; } = default!;
        
        public string? ApiError { get; set; }

        // Cargar los datos del Agente en el formulario
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            Cliente = await EntityService.GetByIdAsync<Cliente>(client, id, "Cliente");
            if (Cliente == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");

            
            var existingEntity = await EntityService.GetByIdAsync<Cliente>(client, id, "Cliente");
            if (existingEntity == null)
            {
                return NotFound();
            }

            ValueHelper.PreserveOriginalValues(Cliente, existingEntity);

            var response = await client.PutAsJsonAsync($"Cliente/{id}", Cliente);
            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                Cliente = await client.GetFromJsonAsync<Cliente>($"Cliente/{id}");
                return Page();
            }

            // Redirect to the index page
            return RedirectToPage("./Index");
        }

    }
}
