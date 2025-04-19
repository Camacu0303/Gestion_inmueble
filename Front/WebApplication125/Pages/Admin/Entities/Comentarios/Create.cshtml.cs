using System.ComponentModel.DataAnnotations;
using API_WIN_MAIN.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.Comentarios
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public CreateModel(IHttpClientFactory httpClient)
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
            Propiedades = await EntityService.GetAllAsync<Propiedad>(client, "Propiedades");
            Usuarios = await EntityService.GetAllAsync<Usuario>(client, "Usuario");
            Calificaciones = await EntityService.GetAllAsync<Calificacion>(client, "Calificacion");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClient.CreateClient("ApiClient");

            var response = await client.PostAsJsonAsync($"Comentario/", Comentario);

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}