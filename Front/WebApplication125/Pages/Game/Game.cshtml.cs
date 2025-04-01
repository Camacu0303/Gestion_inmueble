using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication125.Models;
using WebApplication125.Services;

namespace WebApplication125.Pages.Game
{
    public class GameModel : PageModel
    {
        public string Mensaje { get; set; } = string.Empty;
        private readonly LeaderboardsService _LeaderboardService;

        [BindProperty]
        public Score Score { get; set; }= new Score();

        public GameModel(LeaderboardsService leaderboards)
        {
            _LeaderboardService = leaderboards;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Mensaje = "Datos Invalidos";
                    return Page();
                }

                Score.UserId = (int) HttpContext.Session.GetInt32("UserIdSession");
                var response = await _LeaderboardService.AddScores(Score);
                if (response)
                {
                    return RedirectToPage("LeaderBoards");
                }
                else
                {
                    Mensaje = "Error en el agregado del puntaje" + response.ToString();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error " + ex.Message;
                return Page();
            }
        }
    }
}
