namespace TrainsOnline.Infrastructure.UserManager
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using TrainsOnline.Application.Helpers;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Domain.Entities;

    public class UserManagerService : IUserManagerService
    {
        private readonly PasswordHasher _passwordHasher = new PasswordHasher();

        public UserManagerService()
        {

        }

        public async Task<User> SetPassword(User user, string? password, CancellationToken cancellationToken = default)
        {
            await new PasswordValidator().ValidateAndThrowAsync(password, cancellationToken: cancellationToken);

            user.Password = _passwordHasher.CreateHash(password!);

            return user;
        }

        public async Task<bool> ValidatePassword(User user, string? password, CancellationToken cancellationToken = default)
        {
            await new PasswordValidator().ValidateAndThrowAsync(password, cancellationToken: cancellationToken);

            return _passwordHasher.ValidatePassword(password!, user.Password);
        }
    }
}
