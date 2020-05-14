namespace TrainsOnline.Application.Handlers.AuthenticationHandlers.Queries.GetResetPasswordToken
{
    using Application.Constants;
    using FluentValidation;

    public class GetResetPasswordTokenQueryValidator : AbstractValidator<SendResetPasswordRequest>
    {
        public GetResetPasswordTokenQueryValidator()
        {
            RuleFor(x => x.Email).NotEmpty()
                                 .WithMessage(ValidationMessages.Email.IsEmpty);
            RuleFor(x => x.Email).EmailAddress()
                                 .WithMessage(ValidationMessages.Email.HasWrongFormat);
        }
    }
}
