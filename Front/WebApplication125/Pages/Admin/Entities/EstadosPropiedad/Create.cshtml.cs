using System.ComponentModel.DataAnnotations;
using API_WIN_MAIN.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.EstadosPropiedad
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public CreateModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public EstadoPropiedad EstadoPropiedad { get; set; } = default!;


        public string? ApiError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");

            var response = await client.PostAsJsonAsync($"EstadoPropiedad/", EstadoPropiedad);

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}