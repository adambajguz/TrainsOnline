namespace TrainsOnline.Infrastructure.CurrentUser
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Domain.Jwt;

    public class CurrentUserService : ICurrentUserService
    {
        public Guid? UserId { get; }
        public bool IsAuthenticated { get; }
        public bool IsAdmin { get; }

        private readonly IHttpContextAccessor _context;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor;

            UserId = GetUserIdFromContext(_context);
            IsAuthenticated = UserId != null;
            IsAdmin = HasRole(Roles.Admin);
        }

        public bool HasRole(string role)
        {
            if (!Roles.IsValidRole(role))
                return false;

            ClaimsIdentity? identity = _context.HttpContext.User.Identity as ClaimsIdentity;
            Claim? result = identity?.FindAll(ClaimTypes.Role).Where(x => x.Value == role).FirstOrDefault();

            return result != null;
        }

        public string[] GetRoles()
        {
            ClaimsIdentity? identity = _context.HttpContext.User.Identity as ClaimsIdentity;
            string[] roles = identity?.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray() ?? new string[] { };

            return roles;
        }

        public static Guid? GetUserIdFromContext(IHttpContextAccessor context)
        {
            ClaimsIdentity? identity = context.HttpContext.User.Identity as ClaimsIdentity;
            Claim? claim = identity?.FindFirst(ClaimTypes.UserData);

            return claim == null ? null : (Guid?)Guid.Parse(claim.Value);
        }
    }
}
