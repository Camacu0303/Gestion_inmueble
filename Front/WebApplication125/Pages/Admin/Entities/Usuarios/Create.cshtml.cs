using System.ComponentModel.DataAnnotations;
using API_WIN_MAIN.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Usuarios
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public CreateModel(IHttpClientFactory httpClient)
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
            Roles = await EntityService.GetAllAsync<Rol>(client, "Rol");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            Usuario.Contraseña = "Default";
            var response = await client.PostAsJsonAsync($"Usuario/", Usuario);
            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                return Page();
            }
            return RedirectToPage("./Index");
        }
    }
}