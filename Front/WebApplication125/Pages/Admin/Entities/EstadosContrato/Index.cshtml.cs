using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Models;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.EstadosContrato
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;

        public IndexModel(IHttpClientFactory httpClient, ILogger<IndexModel> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public List<EstadoContrato> EstadosContrato { get; set; } = new List<EstadoContrato>();
        
        public async Task OnGetAsync()
        {
            try
            {
                var client = _httpClient.CreateClient("ApiClient");
                EstadosContrato = await EntityService.GetAllAsync<EstadoContrato>(client, "EstadoContrato");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar contratos");
            }
        }
    }
}
