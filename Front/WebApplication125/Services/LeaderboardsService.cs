using System.Text.Json;
using System.Text;
using WEB.Models.PreProjectFiles;

namespace WebApplication125.Services
{
    public class LeaderboardsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        public LeaderboardsService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["ApiUrl"] + "Scores/";
           
        }
        public async Task<List<Score>> GetScores()
        {
            var response = await _httpClient.GetAsync(_apiUrl);
            if (!response.IsSuccessStatusCode)
                return new List<Score>();

            var json = await response.Content.ReadAsStringAsync();
            var scores = JsonSerializer.Deserialize<List<Score>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Score>();

            
            return scores.OrderByDescending(p => p.eaten).Take(15).ToList();
        }
        public async Task<bool> AddScores(Score score)
        {
            var jsonScores = JsonSerializer.Serialize(score);
            var content = new StringContent(jsonScores, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_apiUrl, content);

            return response.IsSuccessStatusCode;

        }
    }
}
