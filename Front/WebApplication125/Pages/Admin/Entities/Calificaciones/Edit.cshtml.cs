using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.Calificaciones
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public EditModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Calificacion Calificacion { get; set; } = default!;


        public string? ApiError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            Calificacion = await EntityService.GetByIdAsync<Calificacion>(client, id, "Calificacion");
            if (Calificacion == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");

            var existingEntity = await EntityService.GetByIdAsync<Calificacion>(client, id, "Calificacion");
            if (existingEntity == null)
                return NotFound();

            var response = await client.PutAsJsonAsync($"Calificacion/{id}", Calificacion);

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                Calificacion = await client.GetFromJsonAsync<Calificacion>($"Calificacion/{id}");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
