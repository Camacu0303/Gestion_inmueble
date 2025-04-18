using System.ComponentModel.DataAnnotations;
using API_WIN_MAIN.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;
using System.Reflection;

namespace WEB.Pages.Admin.Entities.Propiedades
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public CreateModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Propiedad Propiedad { get; set; } = default!;
        
        [BindProperty]
        public IFormFile? ImagenFile { get; set; }
        public List<TipoPropiedad>? TiposPropiedad { get; set; }

        public List<EstadoPropiedad>? EstadosPropiedad { get; set; }

        public List<Usuario>? Usuarios { get; set; }
        public string? ApiError { get; set; }
        public async Task<IActionResult> OnGetAsync()

        {
            Propiedad = new Propiedad();
            var client = _httpClient.CreateClient("ApiClient");

            Usuarios = await EntityService.GetAllAsync<Usuario>(client, "Usuario");
            Usuarios.ForEach(p => p.Contraseña = null);

            TiposPropiedad = await EntityService.GetAllAsync<TipoPropiedad>(client, "TipoPropiedad");

            EstadosPropiedad = await EntityService.GetAllAsync<EstadoPropiedad>(client, "EstadoPropiedad");

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            if (ImagenFile != null && ImagenFile.Length > 0)
            {
                var folderName = "CustomerImages";
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImagenFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName, fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImagenFile.CopyToAsync(stream);
                }

                // Guarda la ruta relativa
                Propiedad.imagen_url = $"/{folderName}/{fileName}";
            }
            var response = await client.PostAsJsonAsync($"Propiedades/", Propiedad);
            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                Propiedad = await client.GetFromJsonAsync<Propiedad>($"Propiedades/{id}");
                return Page();
            }
            // Redirect to the index page
            return RedirectToPage("./Index");
        }
    }
}