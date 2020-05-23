namespace TrainsOnline.Application.Handlers.UserHandlers.Commands.CreateUser
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

    public class CreateUserCommand : IRequest<IdResponse>
    {
        public CreateUserRequest Data { get; }

        public CreateUserCommand(CreateUserRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<CreateUserCommand, IdResponse>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;
            private readonly IMapper _mapper;
            private readonly IUserManagerService _userManager;
            private readonly IDataRightsService _drs;

            public Handler(ITrainsOnlineMongoUnitOfWork uow, IMapper mapper, IUserManagerService userManager, IDataRightsService drs)
            {
                _uow = uow;
                _mapper = mapper;
                _userManager = userManager;
                _drs = drs;
            }

            public async Task<IdResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                CreateUserRequest data = request.Data;

                if (data.IsAdmin)
                    _drs.ValidateIsAdmin();

                await new CreateUserCommandValidator(_uow).ValidateAndThrowAsync(data, cancellationToken: cancellationToken);

                User entity = _mapper.Map<User>(data);
                await _userManager.SetPassword(entity, data.Password, cancellationToken);

                await _uow.UsersRepository.AddAsync(entity);

                await _uow.SaveChangesAsync(cancellationToken);

                return new IdResponse(entity.Id);
            }
        }
    }
}
