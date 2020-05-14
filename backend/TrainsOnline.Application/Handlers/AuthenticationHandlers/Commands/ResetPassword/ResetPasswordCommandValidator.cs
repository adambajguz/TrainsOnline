namespace TrainsOnline.Application.Handlers.AuthenticationHandlers.Commands.ResetPassword
{
    using Application.Constants;
    using FluentValidation;
    using TrainsOnline.Domain.Entities;

    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommandValidator.Model>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.Data.Token).NotEmpty()
                                      .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.User.Id).NotEmpty().Must((request, val, token) =>
            {
                return request.User != null;
            }).WithMessage(ValidationMessages.General.IsIncorrectId);
        }

        public class Model
        {
            public ResetPasswordRequest Data { get; set; }
            public User User { get; set; }

            public Model(ResetPasswordRequest data, User user)
            {
                Data = data;
                User = user;
            }
        }
    }
}
