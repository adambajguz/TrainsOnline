namespace TrainsOnline.Application.Handlers.AuthenticationHandlers.Queries.GetResetPasswordToken
{
    using TrainsOnline.Application.DTO;

    public class SendResetPasswordRequest : IDataTransferObject
    {
        public string? Email { get; set; }
    }
}
