namespace TrainsOnline.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using TrainsOnline.Api.CustomMiddlewares.Exceptions;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Handlers.UserHandlers.Commands.ChangePassword;
    using TrainsOnline.Application.Handlers.UserHandlers.Commands.CreateUser;
    using TrainsOnline.Application.Handlers.UserHandlers.Commands.DeleteUser;
    using TrainsOnline.Application.Handlers.UserHandlers.Commands.UpdateUser;
    using TrainsOnline.Application.Handlers.UserHandlers.Queries.GetUserDetails;
    using TrainsOnline.Application.Handlers.UserHandlers.Queries.GetUsersList;
    using TrainsOnline.Domain.Jwt;

    [Route("api/user")]
    [SwaggerTag("Create, update, and get user")]
    public class UserController : BaseController
    {
        public const string Create = nameof(CreateUser);
        public const string GetCurrentDetails = nameof(GetCurrentUserDetails);
        public const string GetDetails = nameof(GetUserDetails);
        public const string Update = nameof(UpdateUser);
        public const string Delete = nameof(DeleteUser);
        public const string ChangePassword = nameof(ChangeUserPassword);
        public const string GetAll = nameof(GetUsersList);

        [HttpPost("create")]
        [SwaggerOperation(
            Summary = "Create (register) a new user",
            Description = "Creates a new user (requires [Admin] role to create admin account)")]
        [SwaggerResponse(StatusCodes.Status200OK, "User created", typeof(IdResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest user)
        {
            return Ok(await Mediator.Send(new CreateUserCommand(user)));
        }

        [Authorize(Roles = Roles.User)]
        [HttpGet("get-current")]
        [SwaggerOperation(
            Summary = "Get authenticated user details [" + Roles.User + "]",
            Description = "Gets authenticated user details based on token")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetUserDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetCurrentUserDetails()
        {
            IdRequest data = new IdRequest((Guid)CurrentUser.UserId!);

            return Ok(await Mediator.Send(new GetUserDetailsQuery(data)));
        }

        [Authorize(Roles = Roles.User)]
        [HttpGet("get/{id:guid}")]
        [SwaggerOperation(
            Summary = "Get user details [" + Roles.User + "]",
            Description = "Gets user details")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetUserDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetUserDetails([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new GetUserDetailsQuery(new IdRequest(id))));
        }

        [Authorize(Roles = Roles.User)]
        [HttpPut("update")]
        [SwaggerOperation(
            Summary = "Updated user details [" + Roles.User + "]",
            Description = "Updates user details (requires [Admin] role to create admin account)")]
        [SwaggerResponse(StatusCodes.Status200OK, "User details updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest user)
        {
            return Ok(await Mediator.Send(new UpdateUserCommand(user)));
        }

        [Authorize(Roles = Roles.User)]
        [HttpGet("delete/{id:guid}")]
        [SwaggerOperation(
            Summary = "Delete user [" + Roles.User + "]",
            Description = "Deletes user")]
        [SwaggerResponse(StatusCodes.Status200OK, "User deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new DeleteUserCommand(new IdRequest(id))));
        }

        [Authorize(Roles = Roles.User)]
        [HttpPatch("change-password")]
        [SwaggerOperation(
            Summary = "Change user password [" + Roles.User + "]",
            Description = "Changes password of an user")]
        [SwaggerResponse(StatusCodes.Status200OK, "Password changed")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordRequest user)
        {
            return Ok(await Mediator.Send(new ChangePasswordCommand(user)));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("get-all")]
        [SwaggerOperation(
            Summary = "Get all users [" + Roles.Admin + "]",
            Description = "Gets a list of all users")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetUsersListResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetUsersList()
        {
            return Ok(await Mediator.Send(new GetUsersListQuery()));
        }
    }
}
