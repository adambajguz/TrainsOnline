namespace TrainsOnline.Api.SoapEndpoints.Interfaces
{
    using System.ServiceModel;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Api.SoapEndpoints.Core;
    using TrainsOnline.Application.Handlers.AuthenticationHandlers.Commands.ResetPassword;
    using TrainsOnline.Application.Handlers.AuthenticationHandlers.Queries.GetResetPasswordToken;
    using TrainsOnline.Application.Handlers.AuthenticationHandlers.Queries.GetValidToken;

    [ServiceContract]
    public interface IAuthenticationSoapEndpointService : ISoapEndpointService
    {
        [OperationContract]
        Task<JwtTokenModel> Login(LoginRequest model);

        [OperationContract]
        Task<string> ResetPasswordStep1(SendResetPasswordRequest request);

        [OperationContract]
        Task<Unit> ResetPasswordStep2(ResetPasswordRequest request);
    }
}