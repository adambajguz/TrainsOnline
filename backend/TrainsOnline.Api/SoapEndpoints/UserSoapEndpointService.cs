namespace TrainsOnline.Api.SoapEndpoints
{
    using System;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Api.SoapEndpoints.Core;
    using TrainsOnline.Api.SoapEndpoints.Interfaces;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Handlers.UserHandlers.Commands.ChangePassword;
    using TrainsOnline.Application.Handlers.UserHandlers.Commands.CreateUser;
    using TrainsOnline.Application.Handlers.UserHandlers.Commands.DeleteUser;
    using TrainsOnline.Application.Handlers.UserHandlers.Commands.UpdateUser;
    using TrainsOnline.Application.Handlers.UserHandlers.Queries.GetUserDetails;
    using TrainsOnline.Application.Handlers.UserHandlers.Queries.GetUsersList;
    using TrainsOnline.Application.Interfaces;

    [SoapRoute("[baseUrl]/user", "User", "Create, update, and get user")]
    public class UserSoapEndpointService : IUserSoapEndpointService
    {
        protected IMediator Mediator { get; }
        protected ICurrentUserService CurrentUser { get; }

        public UserSoapEndpointService(IMediator mediator, ICurrentUserService currentUser)
        {
            Mediator = mediator;
            CurrentUser = currentUser;
        }

        public async Task<IdResponse> CreateUser(CreateUserRequest user)
        {
            return await Mediator.Send(new CreateUserCommand(user));
        }

        public async Task<GetUserDetailsResponse> GetCurrentUserDetails()
        {
            IdRequest data = new IdRequest((Guid)CurrentUser.UserId!);

            return await Mediator.Send(new GetUserDetailsQuery(data));
        }

        public async Task<GetUserDetailsResponse> GetUserDetails(IdRequest id)
        {
            return await Mediator.Send(new GetUserDetailsQuery(id));
        }

        public async Task<Unit> UpdateUser(UpdateUserRequest user)
        {
            return await Mediator.Send(new UpdateUserCommand(user));
        }

        public async Task<Unit> DeleteUser(IdRequest id)
        {
            return await Mediator.Send(new DeleteUserCommand(id));
        }

        public async Task<Unit> ChangePassword(ChangePasswordRequest user)
        {
            return await Mediator.Send(new ChangePasswordCommand(user));
        }

        public async Task<GetUsersListResponse> GetUsersList()
        {
            return await Mediator.Send(new GetUsersListQuery());
        }
    }
}
