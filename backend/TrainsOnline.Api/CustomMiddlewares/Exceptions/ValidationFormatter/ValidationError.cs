namespace TrainsOnline.Api.CustomMiddlewares.Exceptions.ValidationFormatter
{
    public class ValidationError
    {
        public ValidationError(string field, string message, string errorCode)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
            ErrorCode = errorCode;
        }

        public string? Field { get; }

        public string Message { get; }
        public string ErrorCode { get; }
    }
}
