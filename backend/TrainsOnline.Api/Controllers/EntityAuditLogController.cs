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
    using TrainsOnline.Application.Handlers.EntityAuditLog.Commands.CreateRouteLog;
    using TrainsOnline.Application.Handlers.EntityAuditLog.Queries.GetRouteLogDetails;
    using TrainsOnline.Application.Handlers.EntityAuditLog.Queries.GetRouteLogsList;
    using TrainsOnline.Application.Handlers.EntityAuditLogHandlers.Commands.CleanupEntityAuditLog;
    using TrainsOnline.Application.Handlers.RouteHandlers.Queries.GetRouteDetails;
    using TrainsOnline.Domain.Jwt;

    [Route("api/entity-audit-log")]
    [SwaggerTag("Revert entity and get entity audit log")]
    public class EntityAuditLogController : BaseController
    {
        public const string Revert = nameof(RevertEntityUsingAuditLog);
        public const string GetDetails = nameof(GetAuditLogDetails);
        public const string Cleanup = nameof(CleanupAuditLog);
        public const string GetAllForEntity = nameof(GetEntityAuditLogsForEntityList);
        public const string GetAll = nameof(GetEntityAuditLogsList);

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("create")]
        [SwaggerOperation(
            Summary = "Revert entity using audit log [" + Roles.Admin + "]",
            Description = "Revert entity using entity audit log")]
        [SwaggerResponse(StatusCodes.Status200OK, "Route log created", typeof(IdResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> RevertEntityUsingAuditLog([FromBody]RevertUsingEntityAuditLogRequest data)
        {
            return Ok(await Mediator.Send(new RevertUsingEntityAuditLogCommand(data)));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("get/{id:guid}")]
        [SwaggerOperation(
            Summary = "Get audit log details [" + Roles.Admin + "]",
            Description = "Gets audit log details")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetRouteDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetAuditLogDetails([FromRoute]Guid id)
        {
            return Ok(await Mediator.Send(new GetEntityAuditLogDetailsQuery(new IdRequest(id))));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("delete")]
        [SwaggerOperation(
            Summary = "Delete audit log [" + Roles.Admin + "]",
            Description = "Deletes route log")]
        [SwaggerResponse(StatusCodes.Status200OK, "Route log deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> CleanupAuditLog([FromBody]CleanupEntityAuditLogRequest data)
        {
            return Ok(await Mediator.Send(new CleanupEntityAuditLogCommand(data)));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("get-all-for-entity/{id:guid}")]
        [SwaggerOperation(
            Summary = "Get all route logs for entity [" + Roles.Admin + "]",
            Description = "Gets a list of all entity audit logs for specified entity")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetEntityAuditLogsForEntityListResponse))]
        public async Task<IActionResult> GetEntityAuditLogsForEntityList([FromRoute]Guid id)
        {
            return Ok(await Mediator.Send(new GetEntityAuditLogsForEntityListQuery(new IdRequest(id))));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("get-all")]
        [SwaggerOperation(
            Summary = "Get all route logs [" + Roles.Admin + "]",
            Description = "Gets a list of all entity audit logs")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetEntityAuditLogsListResponse))]
        public async Task<IActionResult> GetEntityAuditLogsList()
        {
            return Ok(await Mediator.Send(new GetEntityAuditLogsListQuery()));
        }
    }
}
