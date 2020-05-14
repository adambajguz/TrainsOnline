namespace TrainsOnline.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ForbiddenException : Exception
    {
        public ForbiddenException()
            : base()
        {

        }

        protected ForbiddenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
