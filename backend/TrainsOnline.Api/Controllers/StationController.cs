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
    using TrainsOnline.Application.Handlers.StationHandlers.Commands.CreateStation;
    using TrainsOnline.Application.Handlers.StationHandlers.Commands.DeleteStation;
    using TrainsOnline.Application.Handlers.StationHandlers.Commands.UpdateStation;
    using TrainsOnline.Application.Handlers.StationHandlers.Queries.GetStationDetails;
    using TrainsOnline.Application.Handlers.StationHandlers.Queries.GetStationsList;
    using TrainsOnline.Domain.Jwt;

    [SwaggerTag("Create, update, and get station")]
    public class StationController : BaseController
    {
        public const string Create = nameof(CreateStation);
        public const string GetDetails = nameof(GetStationDetails);
        public const string Update = nameof(UpdateStation);
        public const string Delete = nameof(DeleteStation);
        public const string GetAll = nameof(GetStationsList);

        public StationController()
        {

        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("/api/station/create", Name = Create)]
        [SwaggerOperation(
            Summary = "Create new station [" + Roles.Admin + "]",
            Description = "Creates a new station")]
        [SwaggerResponse(StatusCodes.Status200OK, "Station created", typeof(IdResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> CreateStation([FromBody]CreateStationRequest station)
        {
            return Ok(await Mediator.Send(new CreateStationCommand(station)));
        }

        [HttpGet("/api/station/get/{id:guid}", Name = GetDetails)]
        [SwaggerOperation(
            Summary = "Get station details",
            Description = "Gets station details")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetStationDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetStationDetails([FromRoute]Guid id)
        {
            return Ok(await Mediator.Send(new GetStationDetailsQuery(new IdRequest(id))));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("/api/station/update", Name = Update)]
        [SwaggerOperation(
            Summary = "Updated station details [" + Roles.Admin + "]",
            Description = "Updates station details")]
        [SwaggerResponse(StatusCodes.Status200OK, "Station details updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> UpdateStation([FromBody]UpdateStationRequest station)
        {
            return Ok(await Mediator.Send(new UpdateStationCommand(station)));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("/api/station/delete/{id:guid}", Name = Delete)]
        [SwaggerOperation(
            Summary = "Delete station [" + Roles.Admin + "]",
            Description = "Deletes station")]
        [SwaggerResponse(StatusCodes.Status200OK, "Station deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> DeleteStation([FromRoute]Guid id)
        {
            return Ok(await Mediator.Send(new DeleteStationCommand(new IdRequest(id))));
        }

        [HttpGet("/api/station/get-all", Name = GetAll)]
        [SwaggerOperation(
            Summary = "Get all stations",
            Description = "Gets a list of all stations")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetStationsListResponse))]
        public async Task<IActionResult> GetStationsList()
        {
            return Ok(await Mediator.Send(new GetStationsListQuery()));
        }
    }
}
