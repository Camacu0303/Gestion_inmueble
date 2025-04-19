using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.Roles
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public EditModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Rol Rol { get; set; } = default!;


        public string? ApiError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            Rol = await EntityService.GetByIdAsync<Rol>(client, id, "Rol");
            if (Rol == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");

            var existingEntity = await EntityService.GetByIdAsync<Rol>(client, id, "Rol");
            if (existingEntity == null)
                return NotFound();

            var response = await client.PutAsJsonAsync($"Rol/{id}", Rol);

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                Rol = await client.GetFromJsonAsync<Rol>($"Rol/{id}");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
