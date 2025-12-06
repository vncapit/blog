

using System.Net;

namespace BlogApi.CustomExceptions;

public class RequestException : Exception
{
    public HttpStatusCode Code { get; set; }
    public object? Error { get; set; }
    public RequestException(HttpStatusCode code, string message, object? error = null) : base(message)
    {
        Code = code;
        Error = error;
    }
}