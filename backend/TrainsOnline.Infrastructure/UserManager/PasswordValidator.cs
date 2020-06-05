namespace TrainsOnline.Infrastructure.UserManager
{
    using Application.Constants;
    using FluentValidation;
    using TrainsOnline.Common;

    public class PasswordValidator : AbstractValidator<string?>
    {
        public PasswordValidator()
        {
            RuleFor(x => x).NotEmpty()
                           .WithMessage(ValidationMessages.Password.IsEmpty);
            RuleFor(x => x).MinimumLength(GlobalAppConfig.MIN_PASSWORD_LENGTH)
                           .WithMessage(string.Format(ValidationMessages.Password.IsTooShort, GlobalAppConfig.MIN_PASSWORD_LENGTH));
            RuleFor(x => x).MaximumLength(GlobalAppConfig.MAX_PASSWORD_LENGTH)
                           .WithMessage(string.Format(ValidationMessages.Password.IsTooLong, GlobalAppConfig.MAX_PASSWORD_LENGTH));
        }
    }
}
