using System.ComponentModel.DataAnnotations;
//using API_WIN_MAIN.DTOs;
using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using WEB.Models;

namespace WEB.Pages.Propiedades
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(IHttpClientFactory httpClient, ILogger<CreateModel> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        [BindProperty]
        public PropiedadCreateDto Propiedad { get; set; } = new();

        public SelectList TiposPropiedad { get; set; }
        public SelectList EstadosPropiedad { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            await CargarDropdowns();
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

                // Asignar usuario autenticado (ejemplo temporal)
                Propiedad.id_Usuario = int.Parse(User.FindFirst("sub")?.Value ?? "4");

                var response = await client.PostAsJsonAsync("Propiedades", Propiedad);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Propiedad creada exitosamente";
                    return RedirectToPage("./Index");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Error al crear propiedad. Status: {response.StatusCode}. Response: {errorContent}");

                ErrorMessage = "Error al guardar la propiedad. Detalles: " + errorContent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear propiedad");
                ErrorMessage = "Ocurrió un error inesperado. Por favor intente nuevamente.";
            }

            await CargarDropdowns();
            return Page();
        }

        private async Task CargarDropdowns()
        {
            try
            {
                var client = _httpClient.CreateClient("ApiClient");

                var tiposResponse = await client.GetAsync("TipoPropiedad");
                if (tiposResponse.IsSuccessStatusCode)
                {
                    var tipos = await tiposResponse.Content.ReadFromJsonAsync<List<TipoPropiedad>>();
                    TiposPropiedad = new SelectList(tipos ?? new(), "id_Tipo", "Nombre");
                }

                var estadosResponse = await client.GetAsync("EstadoPropiedad");
                if (estadosResponse.IsSuccessStatusCode)
                {
                    var estados = await estadosResponse.Content.ReadFromJsonAsync<List<EstadoPropiedad>>();
                    EstadosPropiedad = new SelectList(estados ?? new(), "id_Estado", "Nombre");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar dropdowns");
                TiposPropiedad = new SelectList(new List<TipoPropiedad>(), "id_Tipo", "Nombre");
                EstadosPropiedad = new SelectList(new List<EstadoPropiedad>(), "id_Estado", "Nombre");
            }
        }
    }
    public class PropiedadCreateDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La dirección es requerida")]
        [StringLength(255, ErrorMessage = "Máximo 255 caracteres")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "Seleccione un tipo")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un tipo válido")]
        public int id_Tipo { get; set; }

        [Required(ErrorMessage = "Seleccione un estado")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un estado válido")]
        public int id_Estado { get; set; }

        public int id_Usuario { get; set; }
    }
}