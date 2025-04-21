using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using WEB.Models.Credentials;
using API_WIN_MAIN.DTOs.Credentials;
using API_WIN_MAIN.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

namespace WebApplication125.Pages.Auth
{
    public class PasswordRecoveryModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public PasswordRecoveryModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }
        public bool TokenValido { get; set; }
        private String Token { get; set; }
        [BindProperty]
        public String repetirContraseña { get; set; }= String.Empty;
        public string? MensajeError { get; set; }
        [BindProperty]
        public new ResetUser User { get; set; }= new ResetUser();
        private UsuarioDTO UserDTO { get; set; }=new UsuarioDTO();
        public async Task<IActionResult> OnGetAsync(string? token)
        {
            if (string.IsNullOrEmpty(token))
            {
                MensajeError = "Token Inválido o no presente";
                TokenValido = false;
                return Page();
            }
            TokenValido = await ValidarTokenAsync(token);
          
            if (!TokenValido)
            {
                MensajeError = "El token no es válido o ha expirado.";
            }
            User.Token = token;
            User.email = UserDTO.Email;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
         
            if (string.IsNullOrEmpty(User?.Token) || !await ValidarTokenAsync(User.Token))
            {
                TokenValido = false;
                MensajeError = "El token no es válido o ha expirado.";
                return Page();
            }

            if (User.contraseña != repetirContraseña)
            {
                TokenValido = true; 
                MensajeError = "Las contraseñas no coinciden.";
                return Page();
            }
            var response = await _httpClient.PostAsJsonAsync("Auth/Login/update-password", User);
            if (!response.IsSuccessStatusCode)
            {
                TokenValido = true;
                MensajeError = "Ocurrió un error al cambiar la contraseña. Inténtalo de nuevo.";
                return Page();
            }
            return RedirectToPage("./PasswordChanged");
        }


        public async Task<bool> ValidarTokenAsync(String token)
        {
            var response = await _httpClient.PostAsJsonAsync<String>($"Auth/Login/reset-password", token);

            if (!response.IsSuccessStatusCode)
                return false;

            var contenido = await response.Content.ReadFromJsonAsync<UsuarioDTO>();

            if (contenido is null)
                return false;
            UserDTO = contenido;
            return true;
        }
        public class UsuarioDTO
        {
            public string Nombre { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
        }

    }
}