namespace TrainsOnline.Application.Handlers.RouteHandlers.Commands.CreateRoute
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Entities;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.UoW;

    public class CreateRouteCommand : IRequest<IdResponse>
    {
        public CreateRouteRequest Data { get; }

        public CreateRouteCommand(CreateRouteRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<CreateRouteCommand, IdResponse>
        {
            private readonly ITrainsOnlineSQLUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(ITrainsOnlineSQLUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<IdResponse> Handle(CreateRouteCommand request, CancellationToken cancellationToken)
            {
                CreateRouteRequest data = request.Data;

                await new CreateRouteCommandValidator(_uow).ValidateAndThrowAsync(data, cancellationToken: cancellationToken);

                Route entity = _mapper.Map<Route>(data);
                _uow.Routes.Add(entity);

                await _uow.SaveChangesAsync(cancellationToken);

                return new IdResponse(entity.Id);
            }
        }
    }
}
