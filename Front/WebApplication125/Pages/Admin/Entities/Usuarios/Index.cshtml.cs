using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Models;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.Usuarios
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;

        public IndexModel(IHttpClientFactory httpClient, ILogger<IndexModel> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public List<Usuario>? Usuarios { get; set; } = new List<Usuario>();
        public List<Rol>? Roles { get; set; } = new List<Rol>();
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var client = _httpClient.CreateClient("ApiClient");
                Usuarios = await EntityService.GetAllAsync<Usuario>(client, "Usuario");
                Roles = await EntityService.GetAllAsync<Rol>(client, "Rol");
                if (Usuarios == null)
                {
                    return Page();
                }
                Usuarios = Usuarios.Select(a => new Usuario
                {
                    Email = a.Email,
                    id_Usuario = a.id_Usuario,
                    id_Rol = a.id_Rol,
                    Nombre = a.Nombre,
                    Rol = a.Rol,
                    Contraseña = String.Empty
                }).ToList();
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
                return Page();
            }
        }



        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var client = _httpClient.CreateClient("ApiClient");
                var response = await client.DeleteAsync($"Agente/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Agente eliminado correctamente";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al eliminar el contrato";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar contrato");
                TempData["ErrorMessage"] = "Error interno al eliminar el contrato";
            }

            return RedirectToPage();
        }
    }
}
