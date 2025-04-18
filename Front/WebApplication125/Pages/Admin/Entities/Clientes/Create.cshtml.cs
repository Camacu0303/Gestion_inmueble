using System.ComponentModel.DataAnnotations;
using API_WIN_MAIN.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.DTOs.AgenteDto;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.Clientes
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public CreateModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }
        [BindProperty]
        public Cliente Cliente { get; set; } = default!;
        public string? ApiError { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync($"Cliente/", Cliente);
            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                // Reload the agent data on failure to repopulate the form
                Cliente = await client.GetFromJsonAsync<Cliente>($"Cliente/{id}");
                return Page();
            }
            // Redirect to the index page
            return RedirectToPage("./Index");
        }
    }
}