namespace TrainsOnline.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using TrainsOnline.Api.CustomMiddlewares.Exceptions;
    using TrainsOnline.Application.Handlers.AnalyticsRecordHandlers.Commands.DeleteOldAnalyticsRecords;
    using TrainsOnline.Application.Handlers.AnalyticsRecordHandlers.Queries.GetRouteReportsList;
    using TrainsOnline.Domain.Jwt;

    [Route("api/analytics-record")]
    [SwaggerTag("Get analytics records")]
    public class AnalyticsRecordController : BaseController
    {
        public const string DeleteOld = nameof(DeleteOldAnalyticsRecords);
        public const string GetAll = nameof(GetAnalyticsRecordsList);

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("delete-old")]
        [SwaggerOperation(
            Summary = "Delete old analytics records [" + Roles.Admin + "]",
            Description = "Deletes all analytics records with Timestamp lower or equal to specified date. If Date is null then deletes all.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Route logs deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> DeleteOldAnalyticsRecords([FromBody] DeleteOldAnalyticsRecordsRequest data)
        {
            return Ok(await Mediator.Send(new DeleteOldAnalyticsRecordsCommand(data)));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("get-all")]
        [SwaggerOperation(
            Summary = "Get all analytics records [" + Roles.Admin + "]",
            Description = "Gets a list of all analytics records")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetAnalyticsRecordsListQuery))]
        public async Task<IActionResult> GetAnalyticsRecordsList()
        {
            return Ok(await Mediator.Send(new GetAnalyticsRecordsListQuery()));
        }
    }
}
