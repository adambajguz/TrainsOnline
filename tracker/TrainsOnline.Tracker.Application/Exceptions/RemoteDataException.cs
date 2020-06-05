namespace TrainsOnline.Tracker.Application.Exceptions
{
    using System;
    using TrainsOnline.Tracker.Application.DTO;
    using TrainsOnline.Tracker.Application.Extensions;

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
