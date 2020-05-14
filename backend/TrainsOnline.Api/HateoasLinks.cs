namespace TrainsOnline.Api
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Sciensoft.Hateoas.Extensions;
    using TrainsOnline.Api.Controllers;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Handlers.RouteHandlers.Queries.GetRouteDetails;
    using TrainsOnline.Application.Handlers.StationHandlers.Queries.GetStationDetails;
    using TrainsOnline.Application.Handlers.TicketHandlers.Queries.GetTicketDetails;

    public static class HateoasLinks
    {
        public static IServiceCollection AddCustomHateoas(this IServiceCollection services)
        {
            services.AddLink(c =>
            {
                c.AddPolicy<GetStationDetailsResponse>(model =>
                {
                    model.AddSelf(m => m.Id, "This is a GET self link.")
                         .AddCustomPath(m => "/api/station/create", StationController.Create, HttpMethods.Post)
                         .AddCustomPath(m => "/api/station/update", StationController.Update, HttpMethods.Put)
                         .AddCustomPath(m => $"/api/station/delete/{m.Id}", StationController.Delete, HttpMethods.Delete)
                         .AddCustomPath(m => "/api/station/get-all", StationController.GetAll, HttpMethods.Get);
                });

                c.AddPolicy<GetRouteDetailsResponse>(model =>
                {
                    model.AddSelf(m => m.Id, "This is a GET self link.")
                         .AddCustomPath(m => "/api/route/create", RouteController.Create, HttpMethods.Post)
                         .AddCustomPath(m => "/api/route/update", RouteController.Update, HttpMethods.Put)
                         .AddCustomPath(m => $"/api/station/delete/{m.Id}", RouteController.Delete, HttpMethods.Delete)
                         .AddCustomPath(m => "/api/route/get-filtered", RouteController.GetFiltered, HttpMethods.Get)
                         .AddCustomPath(m => "/api/route/get-all", RouteController.GetAll, HttpMethods.Get);
                });

                c.AddPolicy<GetTicketDetailsResponse>(model =>
                {
                    model.AddSelf(m => m.Id, "This is a GET self link.")
                         .AddCustomPath(m => "/api/ticket/create", TicketController.Create, HttpMethods.Post)
                         .AddCustomPath(m => "/api/ticket/update", TicketController.Update, HttpMethods.Put)
                         .AddCustomPath(m => $"/api/station/delete/{m.Id}", TicketController.Delete, HttpMethods.Delete)
                         .AddCustomPath(m => $"/api/ticket/get-document/{m.Id}", TicketController.GetDocument, HttpMethods.Get)
                         .AddCustomPath(m => "/api/ticket/get-all-current-user", TicketController.GetCurrentUser, HttpMethods.Get)
                         .AddCustomPath(m => $"/api/ticket/get-all-user/{m.Id}", TicketController.GetUser, HttpMethods.Get)
                         .AddCustomPath(m => "/api/ticket/get-all", TicketController.GetAll, HttpMethods.Get);
                });

                c.AddPolicy<IdResponse>(model =>
                {
                    model
                         .AddCustomPath(m => $"/api/route/get/{m.Id}", RouteController.GetDetails, HttpMethods.Get)
                         .AddCustomPath(m => $"/api/route/delete/{m.Id}", RouteController.Delete, HttpMethods.Delete)

                         .AddCustomPath(m => $"/api/ticket/get/{m.Id}", TicketController.GetDetails, HttpMethods.Get)
                         .AddCustomPath(m => $"/api/ticket/delete/{m.Id}", TicketController.Delete, HttpMethods.Delete)

                         .AddCustomPath(m => $"/api/station/get/{m.Id}", StationController.GetDetails, HttpMethods.Get)
                         .AddCustomPath(m => $"/api/station/delete/{m.Id}", StationController.Delete, HttpMethods.Delete)

                         .AddCustomPath(m => $"/api/user/get/{m.Id}", UserController.GetDetails, HttpMethods.Get)
                         .AddCustomPath(m => $"/api/user/delete/{m.Id}", UserController.Delete, HttpMethods.Delete);
                });
            });

            return services;
        }
    }
}
