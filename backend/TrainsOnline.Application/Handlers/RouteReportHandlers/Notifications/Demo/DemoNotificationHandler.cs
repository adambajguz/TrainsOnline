namespace TrainsOnline.Application.Handlers.RouteReportHandlers.Notifications.Demo
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class DemoNotificationHandler : INotificationHandler<DemoNotification>
    {
        public DemoNotificationHandler()
        {

        }

        public Task Handle(DemoNotification notification, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Pong 1");

            //await mediator.Publish(new DemoNotification());


            return Task.CompletedTask;
        }
    }
}
