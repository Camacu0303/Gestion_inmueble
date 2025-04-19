using System.ComponentModel.DataAnnotations;
using API_WIN_MAIN.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.EstadosNotificacion
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public CreateModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public EstadoNotificacion EstadoNotificacion { get; set; } = default!;

        public string? ApiError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");

            var response = await client.PostAsJsonAsync($"EstadoNotificacion/", EstadoNotificacion);

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                EstadoNotificacion = await client.GetFromJsonAsync<EstadoNotificacion>($"EstadoNotificacion/{id}");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}