namespace TrainsOnline.Desktop.Application.Interfaces.RemoteDataProvider
{
    using System;
    using System.Threading.Tasks;
    using TrainsOnline.Desktop.Domain.DTO.User;

    public interface IUserData
    {
        bool IsAuthenticated { get; }

        Task<bool> Login(string email, string password);
        void Logout();
        Task<Guid> Register(CreateUserRequest data);

        //Task<UserDetailsValueObject> GetCurrentUser();
        Task<GetUserDetailsResponse> GetCurrentUser();

        Task UpdateUser(UpdateUserRequest data);
        Task ChangePassword(string currentPassword, string newPassword);
    }
}
