using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using WebApplication125.Models;
using WebApplication125.Services;

namespace WebApplication125.Pages.Auth
{
    public class LoginModel : PageModel
    {
        public string Mensaje { get; set; } = string.Empty;
        private readonly UsuarioService _Usuarioservice;

        [BindProperty]
        public LoginRequest LoginRequest { get; set; }= new LoginRequest();

        public LoginModel(UsuarioService Usuarioservice)
        {
            _Usuarioservice = Usuarioservice;

        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Mensaje = "Datos Invalidos";
                    return Page();
                }

                var response = await _Usuarioservice.LoginUsuarioAsync(LoginRequest);

                if (response.IsSuccessStatusCode)
                {        
                    String content = await response.Content.ReadAsStringAsync();
                    var userResponse = JsonSerializer.Deserialize<UserResponse>(content);            
                    HttpContext.Session.SetInt32("UserIdSession", userResponse.id);
                    HttpContext.Session.SetString("UserSession", userResponse.nombre);
                    HttpContext.Session.SetString("UserPicture", userResponse.picture);
                    return RedirectToPage("../Game/LeaderBoards");
                }
                else
                {
                    Mensaje = "Error en el agregado del usuario: " + response.ToString();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error " + ex.Message;
                return Page();
            }
        }

    }
}
