using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;
using WEB.Models.Credentials;

namespace WebApplication125.Pages.Auth { 
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }
        [BindProperty]
        public LoginUser LoginCredentials { get; set; }

        [BindProperty]
        public NewUser NewCredentials { get; set; }

        public void OnGet()
        {
        }
        public class RolResponse
        {
            public int Id_Rol { get; set; }
            public string Nombre { get; set; }
        }

        public class CredencialUsuario
        {
            public int id_Usuario { get; set; }
            public string Email { get; set; }
            public string Nombre { get; set; }
            public int id_Rol { get; set; }
            public RolResponse Rol { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var formType = Request.Form["FormType"];
            if (formType == "login")
            {
                if (LoginCredentials != null &&
                 !string.IsNullOrWhiteSpace(LoginCredentials.email) &&
                 !string.IsNullOrWhiteSpace(LoginCredentials.contrase�a))
                {
                    var response = await _httpClient.PostAsJsonAsync("Auth/Login", LoginCredentials);
                   if (response.IsSuccessStatusCode)
                    {
                        var userInfo = await response.Content.ReadFromJsonAsync<CredencialUsuario>();

                        var jsonUser = JsonSerializer.Serialize(userInfo);
                        HttpContext.Session.SetString("UserSession", jsonUser);

                        return RedirectToPage("/Index");
                    }

                    else
                    {
                        ModelState.AddModelError(string.Empty, "Credenciales de inicio de sesi�n incorrectas.");
                        return Page();
                    }
                }
            }
            else if (formType == "register")
            {
                if (NewCredentials != null &&
                  !string.IsNullOrWhiteSpace(NewCredentials.email) &&
                  !string.IsNullOrWhiteSpace(NewCredentials.contrase�a) &&
                  !string.IsNullOrWhiteSpace(NewCredentials.nombre))
                {
                    var response = await _httpClient.PostAsJsonAsync("Auth/Register", NewCredentials);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error al registrar nuevo usuario.");
                        return Page();
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "Debes completar el formulario de inicio de sesi�n o registro.");
            return Page();
        }

    }
}