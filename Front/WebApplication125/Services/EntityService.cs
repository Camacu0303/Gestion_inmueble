using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WEB.Util
{
    public static class EntityService
    {
        public static async Task<T?> GetByIdAsync<T>(HttpClient client, int id, string endpoint)
        {
            try
            {
                return await client.GetFromJsonAsync<T>($"{endpoint}/{id}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al obtener {endpoint} con ID {id}: {ex.Message}");
                return default;
            }
        }

        public static async Task<List<T>?> GetAllAsync<T>(HttpClient client, string endpoint)
        {
            try
            {
                return await client.GetFromJsonAsync<List<T>>($"{endpoint}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al obtener lista de {endpoint}: {ex.Message}");
                return default;
            }
        }
    }
}
