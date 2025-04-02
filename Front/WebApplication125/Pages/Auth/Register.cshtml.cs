using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Models.PreProjectFiles;
using WebApplication125.Pages;
using WebApplication125.Services;
namespace WebApplication125.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        public string Mensaje { get; set; } = string.Empty;
        private readonly UsuarioService _Usuarioservice;
      
        [BindProperty]
        public Usuarios _UsuarioModels { get; set; }= new Usuarios();
        public RegisterModel(UsuarioService Usuarioservice)
        {
            _Usuarioservice = Usuarioservice;
            
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                //la equivalencia 
                if (!ModelState.IsValid)
                {
                    Mensaje = "Datos Invalidos";
                    return Page();
                }
                var response = await _Usuarioservice.AddUsuariosAsync(_UsuarioModels);

                if (response)
                {
                    Mensaje = "Usuario agregado";
                    return RedirectToPage("../Index");
                }
                else
                {
                    Mensaje = "Error en el agregado del usuario";
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
