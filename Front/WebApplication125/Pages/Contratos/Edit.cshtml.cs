using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WEB.Pages.Contratos
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;

        public EditModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Contrato Contrato { get; set; } = new Contrato();

        public SelectList Propiedades { get; set; }
        public SelectList Clientes { get; set; }
        public SelectList EstadosContrato { get; set; }
        public string? ApiError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var client = _httpClient.CreateClient("ApiClient");

                // Cargar contrato existente
                var contratoResponse = await client.GetFromJsonAsync<Contrato>($"contratos/{id}");
                if (contratoResponse == null)
                {
                    return NotFound();
                }
                Contrato = contratoResponse;

                await CargarDropdowns();
                return Page();
            }
            catch (Exception ex)
            {
                ApiError = $"Error al cargar datos: {ex.Message}";
                return Page();
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
                var response = await client.PutAsJsonAsync($"contratos/{Contrato.id_Contrato}", Contrato);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Index", new { successMessage = "Contrato actualizado correctamente" });
                }

                ApiError = await response.Content.ReadAsStringAsync();
                await CargarDropdowns();
                return Page();
            }
            catch (Exception ex)
            {
                ApiError = $"Error al guardar: {ex.Message}";
                await CargarDropdowns();
                return Page();
            }
        }

        private async Task CargarDropdowns()
        {
            var client = _httpClient.CreateClient("ApiClient");

            var propiedades = await client.GetFromJsonAsync<List<Propiedad>>("propiedades?estado=activo");
            Propiedades = new SelectList(propiedades ?? new(), "id_Propiedad", "Nombre", Contrato.id_Propiedad);

            var clientes = await client.GetFromJsonAsync<List<Cliente>>("clientes?estado=activo");
            Clientes = new SelectList(clientes ?? new(), "id_Cliente", "Nombre", Contrato.id_Cliente);

            var estados = await client.GetFromJsonAsync<List<EstadoContrato>>("estadoscontrato");
            EstadosContrato = new SelectList(estados ?? new(), "id_Estado", "Nombre", Contrato.id_Estado);
        }
    }
}
