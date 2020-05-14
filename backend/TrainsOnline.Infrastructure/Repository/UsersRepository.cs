namespace TrainsOnline.Infrastructure.Repository
{
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using TrainsOnline.Application.Interfaces.Repository;
    using TrainsOnline.Domain.Entities;

    public class UsersRepository : GenericRepository<User>, IUsersRepository
    {
        public UsersRepository(ICurrentUserService currentUserService,
                               IPKPAppDbContext context,
                               IMapper mapper) : base(currentUserService, context, mapper)
        {

        }

        public async Task<bool> IsEmailInUseAsync(string? email)
        {
            User? user = await _dbSet.Where(x => x.Email.Equals(email)).SingleOrDefaultAsync();

            if (user == null)
                return false;

            return true;
        }
    }
}
