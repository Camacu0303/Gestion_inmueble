using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using WebApplication125.Models;

namespace WebApplication125.Services
{

    public class UsuarioService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public UsuarioService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["ApiUrl"] + "Usuario/";
            //  https://localhost:7114/api/Usuario
        }

        public async Task<List<Usuarios>> GetUsuariosAsync()
        {
            var response = await _httpClient.GetAsync(_apiUrl);
            if (!response.IsSuccessStatusCode)
                return new List<Usuarios>();
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<Usuarios>>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new List<Usuarios>();
        }
        public async Task<HttpResponseMessage> LoginUsuarioAsync(LoginRequest loginRequest)
        {
            var jsonRequest = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_apiUrl + "login", content); 

            return response;
        }

        public async Task<bool> AddUsuariosAsync(Usuarios Usuarios)
        {
            var jsonUsuarios = JsonSerializer.Serialize(Usuarios);
            var content = new StringContent(jsonUsuarios, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_apiUrl, content);

            return response.IsSuccessStatusCode;

        }
        public async Task<bool> DeleteUsuariosAsync(int id)
        {

            var response = await _httpClient.DeleteAsync($"{_apiUrl}{id}");

            return response.IsSuccessStatusCode;

        }
        public async Task<bool> UpdateUsuariosAsync(Usuarios Usuarios)
        {
            var jsonUsuarios = JsonSerializer.Serialize(Usuarios);
            var content = new StringContent(jsonUsuarios, Encoding.UTF8, "application/json");
            //  "ApiUrl": "https://localhost:7114/api/Product/85"
            var response = await _httpClient.PutAsync($"{_apiUrl}{Usuarios.Id}", content);

            return response.IsSuccessStatusCode;
        }

    }
}
