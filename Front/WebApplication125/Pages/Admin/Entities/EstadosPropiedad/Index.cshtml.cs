using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Models;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.EstadosPropiedad
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

        public List<EstadoPropiedad> EstadosPropiedad { get; set; } = new List<EstadoPropiedad>();
        
        public async Task OnGetAsync()
        {
            try
            {
                var client = _httpClient.CreateClient("ApiClient");
                EstadosPropiedad = await EntityService.GetAllAsync<EstadoPropiedad>(client, "EstadoPropiedad");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar estados de Notificación");
            }
        }
    }
}
