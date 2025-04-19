using System.ComponentModel.DataAnnotations;
using API_WIN_MAIN.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Util;

namespace WEB.Pages.Admin.Entities.EstadosPago
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        public CreateModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public EstadoPago EstadoPago { get; set; } = default!;


        public string? ApiError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClient.CreateClient("ApiClient");

            var response = await client.PostAsJsonAsync($"EstadoPago/", EstadoPago);

            if (!response.IsSuccessStatusCode)
            {
                ApiError = await response.Content.ReadAsStringAsync();
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}