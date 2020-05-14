namespace TrainsOnline.Application.Handlers.UserHandlers.Commands.DeleteUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.UoW.Generic;
    using TrainsOnline.Domain.Entities;

    public class DeleteUserCommand : IRequest
    {
        public IdRequest Data { get; }

        public DeleteUserCommand(IdRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<DeleteUserCommand, Unit>
        {
            private readonly IPKPAppDbUnitOfWork _uow;
            private readonly IDataRightsService _drs;

            public Handler(IPKPAppDbUnitOfWork uow, IDataRightsService drs)
            {
                _uow = uow;
                _drs = drs;
            }

            public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;
                await _drs.ValidateUserId(data, x => x.Id);

                User user = await _uow.UsersRepository.GetByIdAsync(data.Id);

                EntityRequestByIdValidator<User>.Model validationModel = new EntityRequestByIdValidator<User>.Model(data, user);
                await new EntityRequestByIdValidator<User>().ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);

                _uow.UsersRepository.Remove(user);
                await _uow.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
