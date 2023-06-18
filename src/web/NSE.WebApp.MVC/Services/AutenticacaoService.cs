using NSE.WebApp.MVC.Models;
using System.Text;
using System.Text.Json;

namespace NSE.WebApp.MVC.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly HttpClient _httpClient;

        public AutenticacaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UsuarioResponse> Login(UsuarioLogin usuarioLogin)
        {
            var loginContent = new StringContent(
                JsonSerializer.Serialize(usuarioLogin), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:44370/api/identidade/autenticar", loginContent);

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<UsuarioResponse>(await response.Content.ReadAsStringAsync(), jsonOptions);
        }

        public async Task<UsuarioResponse> Registro(UsuarioRegistro usuarioRegistro)
        {
            var registroContent = new StringContent(
               JsonSerializer.Serialize(usuarioRegistro), Encoding.UTF8, "application/json");

            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var response = await _httpClient.PostAsync("https://localhost:44370/api/identidade/nova-conta", registroContent);

            return JsonSerializer.Deserialize<UsuarioResponse>(await response.Content.ReadAsStringAsync(), jsonOptions);
        }
    }
}
