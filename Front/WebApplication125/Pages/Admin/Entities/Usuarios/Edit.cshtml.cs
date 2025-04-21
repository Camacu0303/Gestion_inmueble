using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Usuarios
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public EditModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Usuario Usuario { get; set; } = default!;
        
        public List<Rol>? Roles { get; set; }
        public string? ApiError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            Usuario = await EntityService.GetByIdAsync<Usuario>(client, id, "Usuario");

            if (Usuario == null)
            {
                return NotFound();
            }
            else {
                Roles = await EntityService.GetAllAsync<Rol>(client, "Rol");
            }
            return Page();
        } 
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");        
            var existingEntity = await EntityService.GetByIdAsync<Usuario>(client, id, "Usuario");
            if (existingEntity == null)
            {
                return NotFound();
            }
            ValueHelper.PreserveOriginalValues(Usuario, existingEntity);
            var response = await client.PutAsJsonAsync($"Usuario/{id}", Usuario);
            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                // Reload the agent data on failure to repopulate the form
                Usuario = await client.GetFromJsonAsync<Usuario>($"Usuario/{id}");
                return Page();
            }
            // Redirect to the index page
            return RedirectToPage("./Index");
        }

    }
}
