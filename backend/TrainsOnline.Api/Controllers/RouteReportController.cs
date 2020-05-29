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
    using TrainsOnline.Application.Handlers.RouteHandlers.Queries.GetRouteDetails;
    using TrainsOnline.Application.Handlers.RouteReportHandlers.Commands.CreateRouteReport;
    using TrainsOnline.Application.Handlers.RouteReportHandlers.Commands.DeleteRouteReport;
    using TrainsOnline.Application.Handlers.RouteReportHandlers.Queries.GetRouteReportDetails;
    using TrainsOnline.Application.Handlers.RouteReportHandlers.Queries.GetRouteReportsList;
    using TrainsOnline.Domain.Jwt;

    [Route("api/route-report")]
    [SwaggerTag("Create, update, and get report log")]
    public class RouteReportController : BaseController
    {
        public const string Create = nameof(CreateReport);
        public const string GetDetails = nameof(GetReportDetails);
        public const string Delete = nameof(DeleteReport);
        public const string GetAll = nameof(GetReportsList);

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("create/")]
        [SwaggerOperation(
            Summary = "Create new route report [" + Roles.Admin + "]",
            Description = "Creates a new route report")]
        [SwaggerResponse(StatusCodes.Status200OK, "Route log created", typeof(IdResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> CreateReport(IdRequest id)
        {
            return Ok(await Mediator.Send(new CreateRouteReportCommand(id)));
        }

        [HttpGet("get/{id:guid}")]
        [SwaggerOperation(
            Summary = "Get route report details",
            Description = "Gets route report details")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetRouteDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetReportDetails([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new GetRouteReportDetailsQuery(new IdRequest(id))));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("delete/{id:guid}")]
        [SwaggerOperation(
            Summary = "Delete route report [" + Roles.Admin + "]",
            Description = "Deletes route report")]
        [SwaggerResponse(StatusCodes.Status200OK, "Route log deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> DeleteReport([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new DeleteRouteReportCommand(new IdRequest(id))));
        }

        [HttpGet("get-all")]
        [SwaggerOperation(
            Summary = "Get all route reports",
            Description = "Gets a list of all route reports")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetRouteReportsListResponse))]
        public async Task<IActionResult> GetReportsList()
        {
            return Ok(await Mediator.Send(new GetRouteReportsListQuery()));
        }
    }
}
