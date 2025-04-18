using System.ComponentModel.DataAnnotations;
using API_WIN_MAIN.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.DTOs.AgenteDto;
using WEB.Util;

namespace WEB.Pages.Agentes
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public CreateModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Agente Agente { get; set; } = default!;

        public List<Usuario>? Usuarios { get; set; }
        public string? ApiError { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            var client = _httpClient.CreateClient("ApiClient");
            Usuarios = await EntityService.GetAllAsync<Usuario>(client, "Usuario");
            Usuarios.ForEach(p => p.Contraseña = null);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync($"Agente/", Agente);
            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                // Reload the agent data on failure to repopulate the form
                Agente = await client.GetFromJsonAsync<Agente>($"Agente/{id}");
                return Page();
            }
            // Redirect to the index page
            return RedirectToPage("./Index");
        }
    }
}