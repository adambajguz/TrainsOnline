namespace TrainsOnline.Application.Handlers.UserHandlers.Commands.ChangePassword
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Domain.Entities;

    public class ChangePasswordCommand : IRequest
    {
        public ChangePasswordRequest Data { get; }

        public ChangePasswordCommand(ChangePasswordRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<ChangePasswordCommand, Unit>
        {
            private readonly ITrainsOnlineSQLUnitOfWork _uow;
            private readonly IDataRightsService _drs;
            private readonly IUserManagerService _userManager;

            public Handler(ITrainsOnlineSQLUnitOfWork uow, IDataRightsService drs, IUserManagerService userManager)
            {
                _uow = uow;
                _drs = drs;
                _userManager = userManager;
            }

            public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
            {
                ChangePasswordRequest data = request.Data;
                await _drs.ValidateUserId(data, x => x.UserId);

                User user = await _uow.UsersRepository.GetByIdAsync(data.UserId);

                ChangePasswordCommandValidator.Model validationModel = new ChangePasswordCommandValidator.Model(data, user);
                await new ChangePasswordCommandValidator(_userManager).ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);

                await _userManager.SetPassword(user, data.NewPassword, cancellationToken);

                _uow.UsersRepository.Update(user);

                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
