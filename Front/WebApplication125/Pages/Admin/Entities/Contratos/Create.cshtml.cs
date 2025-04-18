using System.ComponentModel.DataAnnotations;
using API_WIN_MAIN.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.Contratos
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public CreateModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Contrato Contrato { get; set; } = default!;

        [BindProperty]
        public String fecha { get; set; }
        public List<Propiedad>? Propiedades { get; set; }
        public List<Cliente>? Clientes { get; set; }
        public List<EstadoContrato>? EstadosContrato { get; set; }

        public string? ApiError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            // Formatear fecha al estilo yyyy-MM-dd
            
            Propiedades = await EntityService.GetAllAsync<Propiedad>(client, "Propiedades");
            Clientes = await EntityService.GetAllAsync<Cliente>(client, "Cliente");
            EstadosContrato = await EntityService.GetAllAsync<EstadoContrato>(client, "EstadoContrato");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");

            if (DateTime.TryParse(fecha, out var fechaParsed))
            {
                Contrato.fecha = fechaParsed;
            }

            var response = await client.PostAsJsonAsync($"Contrato/", Contrato);

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                Contrato = await client.GetFromJsonAsync<Contrato>($"Contrato/{id}");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}