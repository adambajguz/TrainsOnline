namespace TrainsOnline.Tracker.Application.DTO.Authentication
{
    using System;
    using TrainsOnline.Tracker.Application.DTO;

    public class ChangePasswordRequest : IDataTransferObject
    {
        public Guid UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
