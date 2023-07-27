using NSE.WebApi.Core.Usuario;
using System.Net.Http.Headers;

namespace NSE.WebApp.MVC.Services.Handlers
{
    public class HttpClientAuthorizationDelegateHandler : DelegatingHandler
    {
        private readonly IAspnetUser _user;

        public HttpClientAuthorizationDelegateHandler(IAspnetUser user)
        {
            _user = user;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Faço o que quiser com o conteudo da request 
            var authorizationHeader = _user.ObterHttpContext().Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                request.Headers.Add("Authorization", new List<string> { authorizationHeader });
            }

            var token = _user.ObterUserToken();

            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Retorno ao fluxo anterior novamente
            return base.SendAsync(request, cancellationToken);
        }
    }
}
