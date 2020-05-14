namespace TrainsOnline.Api.SoapEndpoints
{
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Api.SoapEndpoints.Core;
    using TrainsOnline.Api.SoapEndpoints.Interfaces;
    using TrainsOnline.Application.Handlers.AuthenticationHandlers.Commands.ResetPassword;
    using TrainsOnline.Application.Handlers.AuthenticationHandlers.Queries.GetResetPasswordToken;
    using TrainsOnline.Application.Handlers.AuthenticationHandlers.Queries.GetValidToken;

    [SoapRoute("[baseUrl]/authentication", "Authentication", "User authentication and password reset")]
    public class AuthenticationSoapEndpointService : IAuthenticationSoapEndpointService
    {
        protected IMediator Mediator { get; }

        public AuthenticationSoapEndpointService(IMediator mediator)
        {
            Mediator = mediator;
        }

        public async Task<JwtTokenModel> Login(LoginRequest model)
        {
            return await Mediator.Send(new GetValidTokenQuery(model));
        }

        public async Task<string> ResetPasswordStep1(SendResetPasswordRequest request)
        {
            return await Mediator.Send(new GetResetPasswordTokenQuery(request));
        }

        public async Task<Unit> ResetPasswordStep2(ResetPasswordRequest request)
        {
            return await Mediator.Send(new ResetPasswordCommand(request));
        }
    }
}
