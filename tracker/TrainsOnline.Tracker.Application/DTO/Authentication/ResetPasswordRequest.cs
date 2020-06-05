namespace TrainsOnline.Tracker.Application.DTO.Authentication
{
    using TrainsOnline.Tracker.Application.DTO;

    public class ResetPasswordRequest : IDataTransferObject
    {
        public string Token { get; set; }
        public string Password { get; set; }
    }
}
