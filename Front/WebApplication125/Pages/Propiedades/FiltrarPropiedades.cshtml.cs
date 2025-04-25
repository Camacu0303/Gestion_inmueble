using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WEB.Models;
using WEB.Util;  // Assuming you have a model for Propiedad

namespace WEB.Pages.Propiedades
{
    public class FiltrarPropiedadesModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<FiltrarPropiedadesModel> _logger;

        public FiltrarPropiedadesModel(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<FiltrarPropiedadesModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }
        public List<TipoPropiedad> tipos { set; get; } = new List<TipoPropiedad>();
        public List<EstadoPropiedad> Estados { set; get; } = new List<EstadoPropiedad>();

        public List<Propiedad> Propiedades { get; set; } = new List<Propiedad>(); // Store the result here

        public async Task OnGet(string? nombre, decimal? precioMin, decimal? precioMax, int? id_tipo, int? id_estado, string? direccion)
        {
            var client = _httpClientFactory.CreateClient();

            // Assuming your API URL is something like this:
            string apiUrl = $"{_configuration["ApiUrl"]}Propiedades/filtros?nombre={nombre}&precioMin={precioMin}&precioMax={precioMax}&id_tipo={id_tipo}&id_estado={id_estado}&direccion={direccion}";

            try
            {
                var client2 = _httpClientFactory.CreateClient("ApiClient");
                Estados = await EntityService.GetAllAsync<EstadoPropiedad>(client2, "EstadoPropiedad");
                tipos = await EntityService.GetAllAsync<TipoPropiedad>(client2, "TipoPropiedad");
                var result = await client.GetFromJsonAsync<List<Propiedad>>(apiUrl);
                if (result != null)
                {
                    Propiedades = result; 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching filtered properties: {ex.Message}");
            }
        }
    }
}
