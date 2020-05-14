namespace TrainsOnline.Desktop.Domain.DTO.Authentication
{
    public class SendResetPasswordRequest : IDataTransferObject
    {
        public string Email { get; set; }
    }
}
