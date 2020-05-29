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
    using TrainsOnline.Application.Handlers.RouteHandlers.Queries.GetFilteredRoutesList;
    using TrainsOnline.Application.Handlers.RouteHandlers.Queries.GetRouteDetails;
    using TrainsOnline.Application.Handlers.RouteHandlers.Queries.GetRoutesList;
    using TrainsOnline.Domain.Jwt;

    [Route("api/route")]
    [SwaggerTag("Create, update, and get route")]
    public class RouteController : BaseController
    {
        public const string Create = nameof(CreateRoute);
        public const string GetDetails = nameof(GetRouteDetails);
        public const string Update = nameof(UpdateRoute);
        public const string Delete = nameof(DeleteRoute);
        public const string GetFiltered = nameof(GetFilteredRoutesList);
        public const string GetAll = nameof(GetRoutesList);

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("create")]
        [SwaggerOperation(
            Summary = "Create new route [" + Roles.Admin + "]",
            Description = "Creates a new route")]
        [SwaggerResponse(StatusCodes.Status200OK, "Route created", typeof(IdResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> CreateRoute([FromBody]CreateRouteRequest route)
        {
            return Ok(await Mediator.Send(new CreateRouteCommand(route)));
        }

        [HttpGet("get/{id:guid}")]
        [SwaggerOperation(
            Summary = "Get route details",
            Description = "Gets route details")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetRouteDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetRouteDetails([FromRoute]Guid id)
        {
            return Ok(await Mediator.Send(new GetRouteDetailsQuery(new IdRequest(id))));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("update")]
        [SwaggerOperation(
            Summary = "Updated route details [" + Roles.Admin + "]",
            Description = "Updates route details")]
        [SwaggerResponse(StatusCodes.Status200OK, "Route details updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> UpdateRoute([FromBody]UpdateRouteRequest route)
        {
            return Ok(await Mediator.Send(new UpdateRouteCommand(route)));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("delete/{id:guid}")]
        [SwaggerOperation(
            Summary = "Delete route [" + Roles.Admin + "]",
            Description = "Deletes route")]
        [SwaggerResponse(StatusCodes.Status200OK, "Route deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> DeleteRoute([FromRoute]Guid id)
        {
            return Ok(await Mediator.Send(new DeleteRouteCommand(new IdRequest(id))));
        }

        [HttpPost("get-filtered")]
        [SwaggerOperation(
            Summary = "Get filtered routes",
            Description = "Gets a filtered list of routes")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetRoutesListResponse))]
        public async Task<IActionResult> GetFilteredRoutesList(GetFilteredRoutesListRequest data)
        {
            return Ok(await Mediator.Send(new GetFilteredRoutesListQuery(data)));
        }

        [HttpGet("get-all")]
        [SwaggerOperation(
            Summary = "Get all routes",
            Description = "Gets a list of all routes")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetRoutesListResponse))]
        public async Task<IActionResult> GetRoutesList()
        {
            return Ok(await Mediator.Send(new GetRoutesListQuery()));
        }
    }
}
