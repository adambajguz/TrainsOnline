namespace TrainsOnline.Application.Handlers.UserHandlers.Commands.ChangePassword
{
    using Application.Constants;
    using FluentValidation;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Domain.Entities;

    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommandValidator.Model>
    {
        public ChangePasswordCommandValidator(IUserManagerService _userManager)
        {
            RuleFor(x => x.Data.UserId).NotEmpty().Must((request, val, token) =>
            {
                return request.User != null;
            }).WithMessage(ValidationMessages.General.IsIncorrectId);

            RuleFor(x => x.Data.OldPassword).MustAsync(async (request, val, token) => await _userManager.ValidatePassword(request.User, val))
                                            .WithMessage(ValidationMessages.Password.OldIsIncorrect);
        }

        public class Model
        {
            public ChangePasswordRequest Data { get; set; }
            public User? User { get; set; }

            public Model(ChangePasswordRequest data, User? user)
            {
                Data = data;
                User = user;
            }
        }
    }
}
