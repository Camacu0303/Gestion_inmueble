using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Models;
using WEB.Pages.Admin.Entities.Propiedades;
using WEB.Util;

namespace WEB.Pages.Propiedades
{
    public class DetallesModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;

        public DetallesModel(IHttpClientFactory httpClient, ILogger<IndexModel> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public Propiedad? Propiedad { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            Propiedad = await EntityService.GetByIdAsync<Propiedad>(client, id, "Propiedades");

            if (Propiedad == null)
                return NotFound();

            return Page();
        }
    }
}
