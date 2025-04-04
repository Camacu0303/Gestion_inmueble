using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB.Models;

namespace WEB.Pages.Propiedades
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;

        public CreateModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Propiedad Propiedad { get; set; } = new Propiedad();

        public SelectList TiposPropiedad { get; set; }
        public SelectList EstadosPropiedad { get; set; }
        public string? ApiError { get; set; }

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
            var response = await client.PostAsJsonAsync("propiedades", Propiedad);

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                await CargarDropdowns();
                return Page();
            }

            return RedirectToPage("./Index");
        }

        private async Task CargarDropdowns()
        {
            var client = _httpClient.CreateClient("ApiClient");

            var tipos = await client.GetFromJsonAsync<List<TipoPropiedad>>("tipospropiedad");
            TiposPropiedad = new SelectList(tipos ?? new(), "id_Tipo", "Nombre");

            var estados = await client.GetFromJsonAsync<List<EstadoPropiedad>>("estadospropiedad");
            EstadosPropiedad = new SelectList(estados ?? new(), "id_Estado", "Nombre");
        }
    }
}