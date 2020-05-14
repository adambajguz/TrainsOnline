namespace TrainsOnline.Application.Handlers.AuthenticationHandlers.Queries.GetValidToken
{
    using Application.Constants;
    using FluentValidation;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Common;
    using TrainsOnline.Domain.Entities;

    public class GetValidTokenQueryValidator : AbstractValidator<GetValidTokenQueryValidator.Model>
    {
        public GetValidTokenQueryValidator(IUserManagerService _userManager)
        {
            RuleFor(x => x.Data.Email).NotEmpty()
                                      .WithMessage(ValidationMessages.Email.IsEmpty);
            RuleFor(x => x.Data.Email).EmailAddress()
                                      .WithMessage(ValidationMessages.Email.HasWrongFormat);

            RuleFor(x => x.Data.Password).NotEmpty()
                                         .WithMessage(ValidationMessages.Password.IsEmpty);
            RuleFor(x => x.Data.Password).MinimumLength(GlobalAppConfig.MIN_PASSWORD_LENGTH)
                                         .WithMessage(string.Format(ValidationMessages.Password.IsTooShort, GlobalAppConfig.MIN_PASSWORD_LENGTH));

            RuleFor(x => x.User).MustAsync(async (request, val, token) =>
            {
                if (val == null)
                    return false;

                return await _userManager.ValidatePassword(val, request.Data.Password);
            }).WithMessage(ValidationMessages.Auth.EmailOrPasswordIsIncorrect);
        }

        public class Model
        {
            public LoginRequest Data { get; set; }
            public User User { get; set; }

            public Model(LoginRequest data, User user)
            {
                Data = data;
                User = user;
            }
        }
    }
}
