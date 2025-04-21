using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Models;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.Visitas
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

        public List<Visita>? Visitas { get; set; } = new List<Visita>();
        public List<Agente>? Agentes { get; set; }
        public async Task OnGetAsync()
        {
            try
            {
                var client = _httpClient.CreateClient("ApiClient");
                Agentes = await EntityService.GetAllAsync<Agente>(client, "Agente");
                Visitas = await EntityService.GetAllAsync<Visita>(client, "Visita");
                try
                {
                    var visitasConAgentes = from visita in Visitas
                                            join agente in Agentes
                                            on visita.id_Agente equals agente.id_Agente
                                            select new Visita
                                            {
                                                id_Visita = visita.id_Visita,
                                                id_Cliente= visita.id_Cliente,
                                                id_Agente = visita.id_Agente,
                                                Cliente= visita.Cliente,
                                                Propiedad= visita.Propiedad,
                                                id_Propiedad= visita.id_Propiedad,
                                                Fecha = visita.Fecha,  
                                                Agente = agente
                                            };

                Visitas = visitasConAgentes.ToList();
                }
                catch (Exception ex) {
                    Agentes = null;
                    Visitas= null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar Visitas");
            }
        }
    }
}
