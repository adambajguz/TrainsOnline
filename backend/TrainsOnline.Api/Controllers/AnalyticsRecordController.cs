namespace TrainsOnline.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using TrainsOnline.Application.Handlers.AnalyticsRecordHandlers.Queries.GetRouteReportsList;
    using TrainsOnline.Domain.Jwt;

    [Route("api/analytics-record")]
    [SwaggerTag("Get analytics records")]
    public class AnalyticsRecordController : BaseController
    {
        public const string GetAll = nameof(GetAnalyticsRecordsList);

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("get-all")]
        [SwaggerOperation(
            Summary = "Get all route logs [" + Roles.Admin + "]",
            Description = "Gets a list of all analytics records")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetAnalyticsRecordsListQuery))]
        public async Task<IActionResult> GetAnalyticsRecordsList()
        {
            return Ok(await Mediator.Send(new GetAnalyticsRecordsListQuery()));
        }
    }
}
