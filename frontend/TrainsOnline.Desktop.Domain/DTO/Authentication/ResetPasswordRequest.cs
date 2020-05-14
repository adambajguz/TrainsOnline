namespace TrainsOnline.Desktop.Domain.DTO.Authentication
{
    using TrainsOnline.Desktop.Domain.DTO;

    public class ResetPasswordRequest : IDataTransferObject
    {
        public string Token { get; set; }
        public string Password { get; set; }
    }
}
