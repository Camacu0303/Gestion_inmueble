using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using WEB.Models.PreProjectFiles;
using WebApplication125.Services;

namespace WebApplication125.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UsuarioService _Usuarioservice;
        

        public IndexModel(UsuarioService Usuarioservice)
        {
            _Usuarioservice = Usuarioservice;
        }

        public List<Usuarios> _UsuarioModel { get; set; } = new();

        public async Task OnGetAsync()
        {
            _UsuarioModel = await _Usuarioservice.GetUsuariosAsync();
        }

    }
}
