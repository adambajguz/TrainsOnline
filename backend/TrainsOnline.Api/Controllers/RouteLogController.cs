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
    using TrainsOnline.Application.Handlers.RouteHandlers.Commands.CreateRoute;
    using TrainsOnline.Application.Handlers.RouteHandlers.Commands.DeleteRoute;
    using TrainsOnline.Application.Handlers.RouteHandlers.Commands.UpdateRoute;
    using TrainsOnline.Application.Handlers.RouteHandlers.Queries.GetRouteDetails;
    using TrainsOnline.Application.Handlers.RouteHandlers.Queries.GetRoutesList;
    using TrainsOnline.Application.Handlers.RouteLogHandlers.Commands.CreateRouteLog;
    using TrainsOnline.Application.Handlers.RouteLogHandlers.Commands.DeleteRouteLog;
    using TrainsOnline.Application.Handlers.RouteLogHandlers.Commands.UpdateRouteLog;
    using TrainsOnline.Application.Handlers.RouteLogHandlers.Queries.GetRouteLogDetails;
    using TrainsOnline.Application.Handlers.RouteLogHandlers.Queries.GetRouteLogsList;
    using TrainsOnline.Domain.Jwt;

    [Route("api/route-log")]
    [SwaggerTag("Create, update, and get route log")]
    public class RouteLogController : BaseController
    {
        public const string Create = nameof(CreateRoute);
        public const string GetDetails = nameof(GetRouteDetails);
        public const string Update = nameof(UpdateRoute);
        public const string Delete = nameof(DeleteRoute);
        public const string GetAll = nameof(GetRoutesList);

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("create")]
        [SwaggerOperation(
            Summary = "Create new route log [" + Roles.Admin + "]",
            Description = "Creates a new route log")]
        [SwaggerResponse(StatusCodes.Status200OK, "Route log created", typeof(IdResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> CreateRoute([FromBody]CreateRouteLogRequest route)
        {
            return Ok(await Mediator.Send(new CreateRouteLogCommand(route)));
        }

        [HttpGet("get/{id:guid}")]
        [SwaggerOperation(
            Summary = "Get route log details",
            Description = "Gets route log details")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetRouteDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetRouteDetails([FromRoute]Guid id)
        {
            return Ok(await Mediator.Send(new GetRouteLogDetailsQuery(new IdRequest(id))));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("update")]
        [SwaggerOperation(
            Summary = "Updated route log details [" + Roles.Admin + "]",
            Description = "Updates route log details")]
        [SwaggerResponse(StatusCodes.Status200OK, "Route details updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> UpdateRoute([FromBody]UpdateRouteLogRequest route)
        {
            return Ok(await Mediator.Send(new UpdateRouteLogCommand(route)));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("delete/{id:guid}")]
        [SwaggerOperation(
            Summary = "Delete route log [" + Roles.Admin + "]",
            Description = "Deletes route log")]
        [SwaggerResponse(StatusCodes.Status200OK, "Route log deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> DeleteRoute([FromRoute]Guid id)
        {
            return Ok(await Mediator.Send(new DeleteRouteLogCommand(new IdRequest(id))));
        }

        [HttpGet("get-all")]
        [SwaggerOperation(
            Summary = "Get all route logs",
            Description = "Gets a list of all route logs")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetRouteLogsListResponse))]
        public async Task<IActionResult> GetRoutesList()
        {
            return Ok(await Mediator.Send(new GetRouteLogsListQuery()));
        }
    }
}
