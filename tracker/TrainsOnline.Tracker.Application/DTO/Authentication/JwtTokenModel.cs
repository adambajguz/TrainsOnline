namespace TrainsOnline.Tracker.Application.DTO.Authentication
{
    using System;
    using TrainsOnline.Tracker.Application.DTO;

    public class JwtTokenModel : IDataTransferObject
    {
        public string Token { get; set; }
        public TimeSpan Lease { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
