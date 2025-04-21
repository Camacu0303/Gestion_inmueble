using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Models;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.Usuarios
{
    public class ResetPassword : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;

        public ResetPassword(IHttpClientFactory httpClient, ILogger<IndexModel> logger, IConfiguration config)
        {
            _httpClient = httpClient;
            _logger = logger;
            _config = config;
        }
        [BindProperty]
        public Usuario Usuario { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var client = _httpClient.CreateClient("ApiClient");
                Usuario = await EntityService.GetByIdAsync<Usuario>(client, id, "Usuario");
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
                return Page();
            }
        }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            try
            {
                if (Usuario!=null && Usuario.Email != null)
                {
                    SmtpClient smtp = new SmtpClient();
                    string WebAddress = _config["LocalUrl"];
                    string mensaje = string.Empty;

                    // Esperar respuesta del endpoint que devuelve un string (hash)
                    string url = $"Auth/Login/quickHash?email={Usuario.Email}";
                    var response = await client.PostAsync(url, null);

                    if (response.IsSuccessStatusCode)
                    {
                        string hash = await response.Content.ReadAsStringAsync();
                        hash = hash.Replace("\"", "");
                        mensaje = EmailTemplates.GenerarResetPasswordHtml(Usuario, WebAddress, hash);

                        // Enviar el correo
                        smtp.EnviarCorreo(Usuario.Email, "Cambio de contraseña", mensaje);
                    }
                    else
                    {
                        // Log o manejo de error
                        string errorMsg = await response.Content.ReadAsStringAsync();
                        throw new Exception($"Error al generar hash: {errorMsg}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Aquí puedes loguear el error
                Console.WriteLine("Error al enviar correo de recuperación: " + ex.Message);
            }
            return RedirectToPage("./Index");
        }
    }
}
