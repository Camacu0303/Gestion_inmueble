using System.ComponentModel.DataAnnotations;
using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB.Models;

namespace WEB.Pages.Propiedades
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<EditModel> _logger;

        public EditModel(IHttpClientFactory httpClient, ILogger<EditModel> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        [BindProperty]
        public PropiedadUpdateDto Propiedad { get; set; }

        public SelectList TiposPropiedad { get; set; }
        public SelectList EstadosPropiedad { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                await CargarDropdowns();

                var client = _httpClient.CreateClient("ApiClient");
                var response = await client.GetAsync($"Propiedades/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = "Error al cargar la propiedad";
                    return RedirectToPage("./Index");
                }

                var propiedad = await response.Content.ReadFromJsonAsync<PropiedadDetailDto>();
                Propiedad = new PropiedadUpdateDto
                {
                    Id = propiedad.Id,
                    Nombre = propiedad.Nombre,
                    Direccion = propiedad.Direccion,
                    Precio = propiedad.Precio,
                    IdTipo = propiedad.IdTipo,
                    IdEstado = propiedad.IdEstado
                };

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cargar propiedad ID: {id} para edición");
                ErrorMessage = "Error interno al cargar la propiedad";
                return RedirectToPage("./Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await CargarDropdowns();
                    return Page();
                }

                var client = _httpClient.CreateClient("ApiClient");
                var response = await client.PutAsJsonAsync($"Propiedades/{Propiedad.Id}", Propiedad);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Propiedad actualizada correctamente";
                    return RedirectToPage("./Index");
                }

                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                ErrorMessage = errorResponse?.message ?? "Error al actualizar la propiedad";
                await CargarDropdowns();
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar propiedad ID: {Propiedad.Id}");
                ErrorMessage = "Error interno al actualizar la propiedad";
                await CargarDropdowns();
                return Page();
            }
        }

        private async Task CargarDropdowns()
        {
            try
            {
                var client = _httpClient.CreateClient("ApiClient");

                var tiposTask = client.GetFromJsonAsync<List<TipoPropiedad>>("TipoPropiedad");
                var estadosTask = client.GetFromJsonAsync<List<EstadoPropiedad>>("EstadoPropiedad");

                await Task.WhenAll(tiposTask, estadosTask);

                TiposPropiedad = new SelectList(tiposTask.Result ?? new(), "id_Tipo", "Nombre");
                EstadosPropiedad = new SelectList(estadosTask.Result ?? new(), "id_Estado", "Nombre");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar dropdowns para edición");
                TiposPropiedad = new SelectList(new List<TipoPropiedad>(), "id_Tipo", "Nombre");
                EstadosPropiedad = new SelectList(new List<EstadoPropiedad>(), "id_Estado", "Nombre");
            }
        }
    }
    public class PropiedadUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La dirección es requerida")]
        [StringLength(255, ErrorMessage = "Máximo 255 caracteres")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El tipo es requerido")]
        public int IdTipo { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        public int IdEstado { get; set; }
    }
    public class PropiedadDetailDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public decimal Precio { get; set; }
        public int IdTipo { get; set; }
        public int IdEstado { get; set; }
        public int IdUsuario { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
    }
}
