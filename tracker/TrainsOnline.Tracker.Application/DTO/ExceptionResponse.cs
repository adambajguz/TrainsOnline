namespace TrainsOnline.Tracker.Application.DTO
{
    using System.Net;

    public class ExceptionResponse
    {
        public HttpStatusCode StatusCode { get; }
        public string Message { get; }
        public object Errors { get; }

        public ExceptionResponse(HttpStatusCode statusCode, string message, object errors)
        {
            StatusCode = statusCode;
            Message = message;
            Errors = errors;
        }
    }
}
