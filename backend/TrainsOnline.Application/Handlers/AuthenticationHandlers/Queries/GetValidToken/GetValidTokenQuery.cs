namespace TrainsOnline.Application.Handlers.AuthenticationHandlers.Queries.GetValidToken
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Jwt;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Application.Interfaces.UoW.Generic;
    using TrainsOnline.Domain.Entities;

    public class GetValidTokenQuery : IRequest<JwtTokenModel>
    {
        public LoginRequest Data { get; }

        public GetValidTokenQuery(LoginRequest login)
        {
            Data = login;
        }

        public class Handler : IRequestHandler<GetValidTokenQuery, JwtTokenModel>
        {
            private readonly IPKPAppDbUnitOfWork _uow;
            private readonly IJwtService _jwt;
            private readonly IUserManagerService _userManager;

            public Handler(IPKPAppDbUnitOfWork uow, IJwtService jwt, IUserManagerService userManager)
            {
                _uow = uow;
                _jwt = jwt;
                _userManager = userManager;
            }

            public async Task<JwtTokenModel> Handle(GetValidTokenQuery request, CancellationToken cancellationToken)
            {
                LoginRequest data = request.Data;

                User user = await _uow.UsersRepository.FirstOrDefaultAsync(x => x.Email.Equals(data.Email));
                GetValidTokenQueryValidator.Model validationModel = new GetValidTokenQueryValidator.Model(data, user);

                await new GetValidTokenQueryValidator(_userManager).ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);

                string[] roles = Roles.BuildArray(Roles.User);
                if (user.IsAdmin)
                    roles = Roles.BuildArray(Roles.User, Roles.Admin);

                return _jwt.GenerateJwtToken(user.Email, user.Id, roles);
            }
        }
    }
}
