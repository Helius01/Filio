using System.Net;
using Filio.Common.ErrorHandler.Abstractions;

namespace Filio.Common.ErrorHandler.RecoverableErrors;

public class HttpError : BaseRecoverableError
{
    public HttpError(string message, HttpStatusCode statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }
    public HttpStatusCode StatusCode { get; init; }
    public string? Message { get; init; }
}