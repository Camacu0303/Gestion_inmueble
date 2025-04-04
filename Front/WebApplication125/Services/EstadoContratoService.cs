using API_WIN_MAIN.Models;
using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using WEB.Models.PreProjectFiles;

namespace WebApplication125.Services
{

    public class EstadoContratoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public EstadoContratoService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["ApiUrl"] + "EstadoContrato/";
            //  https://localhost:7114/api/Usuario
        }

        public async Task<List<EstadoContrato>> GetEstadoContratosAsync()
        {
            var response = await _httpClient.GetAsync(_apiUrl);
            if (!response.IsSuccessStatusCode)
                return new List<EstadoContrato>();
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<EstadoContrato>>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new List<EstadoContrato>();
        }

        public async Task<bool> AddEstadoContratoAsync(EstadoContrato EstadoContratos)
        {
            var jsonEstadoContratos = JsonSerializer.Serialize(EstadoContratos);
            var content = new StringContent(jsonEstadoContratos, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_apiUrl, content);

            return response.IsSuccessStatusCode;

        }
        public async Task<bool> DeleteEstadoContratosAsync(int id)
        {

            var response = await _httpClient.DeleteAsync($"{_apiUrl}{id}");

            return response.IsSuccessStatusCode;

        }
        public async Task<bool> UpdateEstadoContratosAsync(EstadoContrato EstadoContratos)
        {
            var jsonEstadoContratos = JsonSerializer.Serialize(EstadoContratos);
            var content = new StringContent(jsonEstadoContratos, Encoding.UTF8, "application/json");
            //  "ApiUrl": "https://localhost:7114/api/Product/85"
            var response = await _httpClient.PutAsync($"{_apiUrl}{EstadoContratos.id_Estado}", content);

            return response.IsSuccessStatusCode;
        }

    }
}
