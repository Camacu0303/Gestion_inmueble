using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WEB.Pages.Contratos
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;

        public CreateModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Contrato Contrato { get; set; } = new Contrato();

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

            var client = _httpClient.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("Contrato", Contrato);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }

            ModelState.AddModelError(string.Empty, await response.Content.ReadAsStringAsync());
            await CargarDropdowns();
            return Page();
        }

        private async Task CargarDropdowns()
        {
            var client = _httpClient.CreateClient("ApiClient");

            var propiedades = await client.GetFromJsonAsync<List<Propiedad>>("propiedades");
            Propiedades = new SelectList(propiedades ?? new(), "id_Propiedad", "Nombre");

            var clientes = await client.GetFromJsonAsync<List<Cliente>>("clientes");
            Clientes = new SelectList(clientes ?? new(), "id_Cliente", "Nombre");

            var estados = await client.GetFromJsonAsync<List<EstadoContrato>>("estadoscontrato");
            EstadosContrato = new SelectList(estados ?? new(), "id_Estado", "Nombre");
        }
    }
}
