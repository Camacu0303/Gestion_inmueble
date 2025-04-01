using API_WEB_CLIENT.Models;

namespace API_WEB_CLIENT.Services
{
    public class UsuarioService
    {
        private readonly HttpClient _httpClient;

        public UsuarioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Usuario>>("https://localhost:7094/api/Usuario");
            return response ?? new List<Usuario>();
        }

        public async Task<Usuario> GetUsuarioAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<Usuario>($"https://localhost:7094/api/Usuario/{id}");
            return response;
        }

        public async Task AddUsuarioAsync(Usuario usuario)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7094/api/Usuario", usuario);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Usuario creado correctamente");
                }
                else
                {
                    Console.WriteLine($"Error al crear usuario: {response.StatusCode}");
                }
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar el usuario: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateUsuarioAsync(int id, Usuario usuario)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7094/api/Usuario/{id}", usuario);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteUsuarioAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7094/api/Usuario/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
