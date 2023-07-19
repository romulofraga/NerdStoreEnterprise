using System.Net;

namespace NSE.WebApp.MVC.Extensions;

public class CustomHttpRequestException : Exception
{
    public CustomHttpRequestException()
    {
    }

    public CustomHttpRequestException(string message) : base(message)
    {
    }

    public CustomHttpRequestException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public CustomHttpRequestException(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; private set; }
}