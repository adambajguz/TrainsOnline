namespace TrainsOnline.Desktop.Domain.DTO.Authentication
{
    using System;
    using TrainsOnline.Desktop.Domain.DTO;

    public class JwtTokenModel : IDataTransferObject
    {
        public string Token { get; set; }
        public TimeSpan Lease { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
