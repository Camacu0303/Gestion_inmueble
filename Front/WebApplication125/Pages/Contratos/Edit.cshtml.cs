using System.ComponentModel.DataAnnotations;
using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB.Pages.Propiedades;

namespace WEB.Pages.Contratos
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
        public ContratoUpdateDto Contrato { get; set; }

        public SelectList Propiedades { get; set; }
        public SelectList Clientes { get; set; }
        public SelectList EstadosContrato { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await CargarDropdowns();

            try
            {
                var client = _httpClient.CreateClient("ApiClient");
                Contrato = await client.GetFromJsonAsync<ContratoUpdateDto>($"Contrato/{id}");

                if (Contrato == null)
                {
                    return NotFound();
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cargar contrato ID: {id}");
                return RedirectToPage("./Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarDropdowns();
                return Page();
            }

            try
            {
                var client = _httpClient.CreateClient("ApiClient");
                var response = await client.PutAsJsonAsync($"Contrato/{Contrato.Id}", Contrato);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Contrato actualizado correctamente";
                    return RedirectToPage("./Index");
                }

                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Error al actualizar contrato: {error}");
                ModelState.AddModelError(string.Empty, "Error al actualizar el contrato");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar contrato");
                ModelState.AddModelError(string.Empty, "Error interno al actualizar el contrato");
            }

            await CargarDropdowns();
            return Page();
        }

        private async Task CargarDropdowns()
        {
            try
            {
                var client = _httpClient.CreateClient("ApiClient");

                var propiedadesTask = client.GetFromJsonAsync<List<PropiedadDto>>("Propiedades");
                var clientesTask = client.GetFromJsonAsync<List<ClienteDto>>("Cliente");
                var estadosTask = client.GetFromJsonAsync<List<EstadoContratoDto>>("EstadoContrato");

                await Task.WhenAll(propiedadesTask, clientesTask, estadosTask);

                Propiedades = new SelectList(propiedadesTask.Result ?? new(), "Id", "Nombre");
                Clientes = new SelectList(clientesTask.Result ?? new(), "Id", "Nombre");
                EstadosContrato = new SelectList(estadosTask.Result ?? new(), "Id", "Nombre");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar dropdowns");
            }
        }
        public class EstadoContratoDto
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }
        public class ClienteDto
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Email { get; set; }
            public string Telefono { get; set; }
        }
        public class ContratoUpdateDto
        {
            public int Id { get; set; }

            [Required]
            public int IdPropiedad { get; set; }

            [Required]
            public int IdCliente { get; set; }
            public string Mensaje { get; set; }

            [Required]
            public DateTime Fecha { get; set; }
                        
            [Required]
            [Range(0.01, double.MaxValue)]
            public decimal Monto { get; set; }

            [Required]
            public int IdEstado { get; set; }
        }
    }
}
