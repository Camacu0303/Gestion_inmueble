using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication125.Models;
using WebApplication125.Services;

namespace WebApplication125.Pages.Game
{
    public class HomeModel : PageModel
    {
        public string Mensaje { get; set; } = string.Empty;
        private readonly LeaderboardsService _LeaderboardService;
        public List<Score> _Scores { get; set; } = new();
        public HomeModel(LeaderboardsService leaderboards)
        {
            _LeaderboardService = leaderboards;
        }
        public async Task OnGetAsync()
        {
            _Scores = await _LeaderboardService.GetScores();
        }
    }
}
