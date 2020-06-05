namespace TrainsOnline.Tracker.Application.DTO.Authentication
{
    using TrainsOnline.Tracker.Application.DTO;

    public class LoginRequest : IDataTransferObject
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
