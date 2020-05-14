namespace TrainsOnline.Desktop.Domain.DTO.Authentication
{
    using TrainsOnline.Desktop.Domain.DTO;

    public class LoginRequest : IDataTransferObject
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
