using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WebApplication125.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public RegisterModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        [BindProperty]
        public Usuario NuevoUsuario { get; set; } = new Usuario();

        

        public async Task<IActionResult> OnPostAsync()
{
    if (!ModelState.IsValid)
        return Page();

    try
    {
        var response = await _httpClient.PostAsJsonAsync("Usuario", NuevoUsuario);
        
        if (response.IsSuccessStatusCode)
            return RedirectToPage("Login");

        var errorMessage = await response.Content.ReadAsStringAsync();
        ModelState.AddModelError(string.Empty, $"Error de la API: {errorMessage}");
    }
    catch (HttpRequestException ex)
    {
        ModelState.AddModelError(string.Empty, $"Error de conexión: {ex.Message}");
    }

    return Page();
}

    public class Usuario
    {
        public int id_Usuario { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }
    }
    }
}