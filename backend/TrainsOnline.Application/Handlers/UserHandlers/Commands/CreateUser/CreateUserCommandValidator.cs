namespace TrainsOnline.Application.Handlers.UserHandlers.Commands.CreateUser
{
    using Application.Constants;
    using FluentValidation;
    using TrainsOnline.Application.Interfaces.UoW;

    public class CreateUserCommandValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserCommandValidator(ITrainsOnlineSQLUnitOfWork uow)
        {
            RuleFor(x => x.Email).NotEmpty()
                                 .WithMessage(ValidationMessages.Email.IsEmpty);
            RuleFor(x => x.Email).EmailAddress()
                                 .WithMessage(ValidationMessages.Email.HasWrongFormat);

            RuleFor(x => x.Email).MustAsync(async (request, val, token) =>
            {
                bool checkInUse = await uow.UsersRepository.IsEmailInUseAsync(val);

                return !checkInUse;

            }).WithMessage(ValidationMessages.Email.IsInUse);
        }
    }
}
