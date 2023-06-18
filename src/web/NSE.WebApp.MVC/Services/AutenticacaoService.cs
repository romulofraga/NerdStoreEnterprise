using NSE.WebApp.MVC.Models;
using System.Text;
using System.Text.Json;

namespace NSE.WebApp.MVC.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService
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

            if (!TratarErrosResponse(response))
            {
                return new UsuarioResponse
                {
                    ResponseResult = JsonSerializer.Deserialize<ResponseResult>(await response.Content.ReadAsStringAsync(), jsonOptions)
                };
            }

            return JsonSerializer.Deserialize<UsuarioResponse>(await response.Content.ReadAsStringAsync(), jsonOptions);
        }

        public async Task<UsuarioResponse> Registro(UsuarioRegistro usuarioRegistro)
        {
            var registroContent = new StringContent(
               JsonSerializer.Serialize(usuarioRegistro), Encoding.UTF8, "application/json");

            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var response = await _httpClient.PostAsync("https://localhost:44370/api/identidade/nova-conta", registroContent);

            if (!TratarErrosResponse(response))
            {
                return new UsuarioResponse
                {
                    ResponseResult = JsonSerializer.Deserialize<ResponseResult>(await response.Content.ReadAsStringAsync(), jsonOptions)
                };
            }

            return JsonSerializer.Deserialize<UsuarioResponse>(await response.Content.ReadAsStringAsync(), jsonOptions);
        }
    }
}
