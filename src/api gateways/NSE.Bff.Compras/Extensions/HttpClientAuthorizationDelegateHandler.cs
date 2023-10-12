using System.Net.Http.Headers;
using NSE.WebApi.Core.Usuario;

namespace NSE.Bff.Compras.Extensions;

public class HttpClientAuthorizationDelegateHandler : DelegatingHandler
{
    private readonly IAspnetUser _aspnetUser;

    public HttpClientAuthorizationDelegateHandler(IAspnetUser aspnetUser)
    {
        _aspnetUser = aspnetUser;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Faço o que quiser com o conteudo da request 
        var authorizationHeader = _aspnetUser.ObterHttpContext().Request.Headers["Authorization"];

        if (!string.IsNullOrEmpty(authorizationHeader))
            request.Headers.Add("Authorization", new List<string> { authorizationHeader });

        var token = _aspnetUser.ObterUserToken();

        if (token != null) request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Retorno ao fluxo anterior novamente
        return base.SendAsync(request, cancellationToken);
    }
}