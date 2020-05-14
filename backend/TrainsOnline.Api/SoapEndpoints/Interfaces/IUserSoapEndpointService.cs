namespace TrainsOnline.Api.SoapEndpoints.Interfaces
{
    using System.ServiceModel;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Api.CustomMiddlewares.Soap;
    using TrainsOnline.Api.SoapEndpoints.Core;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Handlers.UserHandlers.Commands.ChangePassword;
    using TrainsOnline.Application.Handlers.UserHandlers.Commands.CreateUser;
    using TrainsOnline.Application.Handlers.UserHandlers.Commands.UpdateUser;
    using TrainsOnline.Application.Handlers.UserHandlers.Queries.GetUserDetails;
    using TrainsOnline.Application.Handlers.UserHandlers.Queries.GetUsersList;
    using TrainsOnline.Domain.Jwt;

    [ServiceContract]
    public interface IUserSoapEndpointService : ISoapEndpointService
    {
        [OperationContract]
        [SoapAuthorize(Roles = Roles.User)]
        Task<IdResponse> CreateUser(CreateUserRequest user);

        [OperationContract]
        [SoapAuthorize(Roles = Roles.User)]
        Task<GetUserDetailsResponse> GetCurrentUserDetails();

        [OperationContract]
        [SoapAuthorize(Roles = Roles.User)]
        Task<GetUserDetailsResponse> GetUserDetails(IdRequest id);

        [OperationContract]
        [SoapAuthorize(Roles = Roles.User)]
        Task<Unit> UpdateUser(UpdateUserRequest user);

        [OperationContract]
        [SoapAuthorize(Roles = Roles.User)]
        Task<Unit> DeleteUser(IdRequest id);

        [OperationContract]
        [SoapAuthorize(Roles = Roles.User)]
        Task<Unit> ChangePassword(ChangePasswordRequest user);

        [OperationContract]
        [SoapAuthorize(Roles = Roles.Admin)]
        Task<GetUsersListResponse> GetUsersList();
    }
}