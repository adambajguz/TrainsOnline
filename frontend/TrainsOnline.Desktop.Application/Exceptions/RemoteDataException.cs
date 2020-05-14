namespace TrainsOnline.Desktop.Application.Exceptions
{
    using System;
    using TrainsOnline.Desktop.Application.Extensions;
    using TrainsOnline.Desktop.Domain.DTO;

    public class RemoteDataException : Exception
    {
        public RemoteDataException(string message) : base(message)
        {

        }

        public ExceptionResponse GetResponse()
        {
            try
            {
                return Message.ToObject<ExceptionResponse>();
            }
            catch (Exception)
            {
                return new ExceptionResponse(System.Net.HttpStatusCode.InternalServerError, "Fatal error", null);
            }
        }
    }
}
