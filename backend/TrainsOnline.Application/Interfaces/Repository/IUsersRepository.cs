namespace TrainsOnline.Application.Interfaces.Repository
{
    using System.Threading.Tasks;
    using Application.Interfaces.Repository.Generic;
    using TrainsOnline.Domain.Entities;

    public interface IUsersRepository : IGenericRelationalRepository<User>
    {
        Task<bool> IsEmailInUseAsync(string? email);
    }
}
