using System.ComponentModel.DataAnnotations;
using API_WIN_MAIN.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.DTOs.AgenteDto;

namespace WEB.Pages.Agentes
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(IHttpClientFactory httpClientFactory, ILogger<CreateModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [BindProperty]
        public AgenteCreateDto Agente { get; set; } = new AgenteCreateDto();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var response = await client.PostAsJsonAsync("api/Agente", Agente);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Agente creado exitosamente";
                    return RedirectToPage("./Index");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error al crear agente. Código: {StatusCode}, Respuesta: {Response}",
                               response.StatusCode, errorContent);

                ModelState.AddModelError(string.Empty, "Error al crear agente: " + errorContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear agente");
                ModelState.AddModelError(string.Empty, "Error interno al crear agente");
            }

            return Page();
        }
    }
    }