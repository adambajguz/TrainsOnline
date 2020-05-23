namespace TrainsOnline.Application.Handlers.RouteHandlers.Commands.UpdateRoute
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Entities;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.Interfaces.UoW.Generic;

    public class UpdateRouteCommand : IRequest
    {
        public UpdateRouteRequest Data { get; }

        public UpdateRouteCommand(UpdateRouteRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<UpdateRouteCommand, Unit>
        {
            private readonly IPKPAppDbUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(IPKPAppDbUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(UpdateRouteCommand request, CancellationToken cancellationToken)
            {
                UpdateRouteRequest data = request.Data;

                Route route = await _uow.RoutesRepository.GetByIdAsync(data.Id);

                UpdateRouteCommandValidator.Model validationModel = new UpdateRouteCommandValidator.Model(data, route);
                await new UpdateRouteCommandValidator(_uow).ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);

                _mapper.Map(data, route);
                _uow.RoutesRepository.Update(route);

                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
