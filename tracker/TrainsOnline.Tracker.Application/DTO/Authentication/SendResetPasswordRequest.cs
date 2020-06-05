namespace TrainsOnline.Tracker.Application.DTO.Authentication
{
    public class SendResetPasswordRequest : IDataTransferObject
    {
        public string Email { get; set; }
    }
}
