namespace TrainsOnline.Application.Handlers.UserHandlers.Commands.ChangePassword
{
    using System;
    using TrainsOnline.Application.DTO;

    public class ChangePasswordRequest : IDataTransferObject
    {
        public Guid UserId { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
