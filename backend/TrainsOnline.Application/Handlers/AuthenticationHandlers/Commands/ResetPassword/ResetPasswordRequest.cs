namespace TrainsOnline.Application.Handlers.AuthenticationHandlers.Commands.ResetPassword
{
    using TrainsOnline.Application.DTO;

    public class ResetPasswordRequest : IDataTransferObject
    {
        public string? Token { get; set; }
        public string? Password { get; set; }
    }
}
