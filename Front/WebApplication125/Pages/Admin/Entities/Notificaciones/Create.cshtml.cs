using System.ComponentModel.DataAnnotations;
using API_WIN_MAIN.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.Notificaciones
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public CreateModel(IHttpClientFactory httpClient)
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
            Usuarios = await EntityService.GetAllAsync<Usuario>(client, "Usuario");
            EstadosNotificacion = await EntityService.GetAllAsync<EstadoNotificacion>(client, "EstadoNotificacion");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");

            var response = await client.PostAsJsonAsync($"Notificacion/", Notificacion);

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}