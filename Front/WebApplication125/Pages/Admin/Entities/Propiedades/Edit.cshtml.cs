using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.Propiedades
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;

        public EditModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Propiedad Propiedad { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImagenFile { get; set; }

        public List<EstadoPropiedad>? EstadosPropiedad { get; set; }
        public List<TipoPropiedad>? TiposPropiedad { get; set; }
        public List<Usuario>? Usuarios { get; set; }
        public string? ApiError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            Propiedad = await EntityService.GetByIdAsync<Propiedad>(client, id, "Propiedades");

            if (Propiedad == null)
                return NotFound();

            EstadosPropiedad = await EntityService.GetAllAsync<EstadoPropiedad>(client, "EstadoPropiedad");
            TiposPropiedad = await EntityService.GetAllAsync<TipoPropiedad>(client, "TipoPropiedad");
            Usuarios = await EntityService.GetAllAsync<Usuario>(client, "Usuario");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");

            var existingEntity = await EntityService.GetByIdAsync<Propiedad>(client, id, "Propiedades");
            if (existingEntity == null)
                return NotFound();

            ValueHelper.PreserveOriginalValues(Propiedad, existingEntity);

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
                if (!string.IsNullOrEmpty(existingEntity.imagen_url))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingEntity.imagen_url.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }
                Propiedad.imagen_url = $"/{folderName}/{fileName}";
            }
            var response = await client.PutAsJsonAsync($"Propiedades/{id}", Propiedad);
            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                Propiedad = await client.GetFromJsonAsync<Propiedad>($"Propiedades/{id}");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
