namespace TrainsOnline.Infrastructure.DataRights
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Application.Exceptions;
    using Application.Interfaces;
    using Microsoft.AspNetCore.Http;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Domain.Entities;
    using TrainsOnline.Domain.Jwt;

    public class DataRightsService : IDataRightsService
    {
        public Guid? UserId => _currentUser.UserId;
        public bool IsAuthenticated => _currentUser.IsAuthenticated;
        public bool IsAdmin => _currentUser.IsAdmin;

        private readonly IHttpContextAccessor _context;
        private readonly ICurrentUserService _currentUser;
        private readonly ITrainsOnlineSQLUnitOfWork _uow;

        public DataRightsService(IHttpContextAccessor context, ICurrentUserService currentUserService, ITrainsOnlineSQLUnitOfWork uow)
        {
            _context = context;
            _currentUser = currentUserService;
            _uow = uow;
        }

        public bool HasRole(string role)
        {
            return _currentUser.HasRole(role);
        }

        public string[] GetRoles()
        {
            return _currentUser.GetRoles();
        }

        public async Task ValidateUserId<T>(T model, Expression<Func<T, Guid>> userIdFieldExpression) where T : class
        {
            Func<T, Guid> func = userIdFieldExpression.Compile();
            Guid userId = func(model);

            await ValidateUserId(userId);
        }

        public async Task ValidateUserId(Guid userIdToValidate)
        {
            Guid userId = _currentUser.UserId ?? throw new ForbiddenException();

            if (!_currentUser.IsAdmin && userIdToValidate != userId)
                throw new ForbiddenException();

            User? user = await _uow.Users.GetByIdAsync(userIdToValidate);
            if (user is null)
                throw new BadUserException();
        }

        public void ValidateIsAdmin()
        {
            ValidateHasRole(Roles.Admin);
        }

        public void ValidateHasRole(string role)
        {
            if (!Roles.IsValidRole(role))
                throw new ForbiddenException();

            ClaimsIdentity? identity = _context.HttpContext.User.Identity as ClaimsIdentity;
            Claim? result = identity?.FindAll(ClaimTypes.Role).Where(x => x.Value == role).FirstOrDefault();

            if (result == null)
                throw new ForbiddenException();
        }
    }
}
