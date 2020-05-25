namespace TrainsOnline.Application.Handlers.AuthenticationHandlers.Commands.ResetPassword
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Exceptions;
    using Domain.Jwt;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Domain.Entities;

    public class ResetPasswordCommand : IRequest
    {
        public ResetPasswordRequest Data { get; }

        public ResetPasswordCommand(ResetPasswordRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<ResetPasswordCommand>
        {
            private readonly ITrainsOnlineSQLUnitOfWork _uow;
            private readonly IJwtService _jwt;
            private readonly IUserManagerService _userManager;

            public Handler(ITrainsOnlineSQLUnitOfWork uow, IJwtService jwt, IUserManagerService userManager)
            {
                _uow = uow;
                _jwt = jwt;
                _userManager = userManager;
            }

            public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
            {
                ResetPasswordRequest data = request.Data;

                if (!_jwt.IsTokenStringValid(data.Token) || !_jwt.IsRoleInToken(data.Token, Roles.ResetPassword))
                    throw new ForbiddenException();

                Guid userId = _jwt.GetUserIdFromToken(data.Token!);
                User user = await _uow.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId));

                ResetPasswordCommandValidator.Model validationData = new ResetPasswordCommandValidator.Model(data, user);
                await new ResetPasswordCommandValidator().ValidateAndThrowAsync(validationData, cancellationToken: cancellationToken);
                await _userManager.SetPassword(user, data.Password, cancellationToken);

                _uow.Users.Update(user);
                await _uow.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
