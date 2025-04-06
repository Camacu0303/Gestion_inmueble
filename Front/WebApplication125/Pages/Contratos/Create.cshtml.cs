using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WEB.Pages.Contratos
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
    public ContratoCreateDto Contrato { get; set; }

    public SelectList Propiedades { get; set; }
    public SelectList Clientes { get; set; }
    public SelectList EstadosContrato { get; set; }

    public async Task OnGetAsync()
    {
        await CargarDropdowns();
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
            var response = await client.PostAsJsonAsync("Contrato", Contrato);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Contrato creado correctamente";
                return RedirectToPage("./Index");
            }

            var error = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Error al crear contrato: {error}");
            ModelState.AddModelError(string.Empty, "Error al crear el contrato");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear contrato");
            ModelState.AddModelError(string.Empty, "Error interno al crear el contrato");
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

    public class ContratoCreateDto
    {
        [Required(ErrorMessage = "La propiedad es requerida")]
        public int IdPropiedad { get; set; }

        [Required(ErrorMessage = "El cliente es requerido")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        public DateTime Fecha { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "El monto es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        public int IdEstado { get; set; }
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

    public class PropiedadDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
}