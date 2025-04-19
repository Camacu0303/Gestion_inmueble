using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.Pagos
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public EditModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }
        [BindProperty]
        public Pago Pago { get; set; } = default!;
        [BindProperty]
        public String fecha { get; set; }
        public List<Propiedad>? Propiedades { get; set; }
        public List<Cliente>? Clientes { get; set; }
        public List<EstadoPago>? EstadosPago { get; set; }
        public string? ApiError { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            Pago = await EntityService.GetByIdAsync<Pago>(client, id, "Pago");

            if (Pago == null)
                return NotFound();

            fecha = Pago.fecha.ToString("yyyy-MM-dd");

            Propiedades = await EntityService.GetAllAsync<Propiedad>(client, "Propiedades");
            Clientes = await EntityService.GetAllAsync<Cliente>(client, "Cliente");
            EstadosPago = await EntityService.GetAllAsync<EstadoPago>(client, "EstadoPago");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");

            var existingEntity = await EntityService.GetByIdAsync<Pago>(client, id, "Pago");
            if (existingEntity == null)
                return NotFound();

            if (DateTime.TryParse(fecha, out var fechaParsed))
            {
                Pago.fecha = fechaParsed;
            }

            var response = await client.PutAsJsonAsync($"Pago/{id}", Pago);

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                Pago = await client.GetFromJsonAsync<Pago>($"Pago/{id}");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
