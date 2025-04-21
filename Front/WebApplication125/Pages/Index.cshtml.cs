using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using WEB.Util;

namespace WebApplication125.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;

        public IndexModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public List<Propiedad>? Propiedades { get; set; } = new List<Propiedad>();
        public async Task OnGetAsync()
        {
            var client = _httpClient.CreateClient("ApiClient");
            Propiedades = await EntityService.GetAllAsync<Propiedad>(client, "Propiedades");                
        }

    }
}
