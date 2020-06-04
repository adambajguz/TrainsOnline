namespace TrainsOnline.Application.Handlers.UserHandlers.Queries.GetUserDetails
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Domain.Entities;

    public class GetUserDetailsQuery : IRequest<GetUserDetailsResponse>
    {
        public IdRequest Data { get; }

        public GetUserDetailsQuery(IdRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<GetUserDetailsQuery, GetUserDetailsResponse>
        {
            private readonly ITrainsOnlineSQLUnitOfWork _uow;
            private readonly IMapper _mapper;
            private readonly IDataRightsService _drs;

            public Handler(ITrainsOnlineSQLUnitOfWork uow, IMapper mapper, IDataRightsService drs)
            {
                _uow = uow;
                _mapper = mapper;
                _drs = drs;
            }

            public async Task<GetUserDetailsResponse> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                User? entity = await _uow.Users.SingleByIdOrDefaultAsync(data.Id, cancellationToken);

                EntityRequestByIdValidator<User>.Model validationModel = new EntityRequestByIdValidator<User>.Model(data, entity);
                await new EntityRequestByIdValidator<User>().ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);

                await _drs.ValidateUserId(data, x => x.Id);

                return _mapper.Map<GetUserDetailsResponse>(entity);
            }
        }
    }
}
