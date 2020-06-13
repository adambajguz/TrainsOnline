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
        /*
         * IHttpContextAccessor.HttpContext.User.Identity shows all null properties in CurrentUserService service
         * https://stackoverflow.com/questions/59793111/ihttpcontextaccessor-httpcontext-user-identity-shows-all-null-properties-in-curr
         *
         *      public string UserId { get; }
         *      public CurrentUserService(IHttpContextAccessor httpContextAccessor)
         *      {
         *           _context = httpContextAccessor.HttpContext;
         *           UserId = GetUserIdFromContext(_context);
         *      }
         *
         * Under the ASP.NET MVC framework, the HttpContext (and therefore HttpContext.Session) is not set when the controller class is contructed as you might expect, but it set ("injected") later by the ControllerBuilder class.
         * The CurrentUserService class that comes with the template I'm using tried to read the user claims in the constructor, so it did not work.
         */

        public Guid? UserId => GetUserIdFromContext(_context);
        public bool IsAuthenticated => _context.User.Identity.IsAuthenticated;
        public bool IsAdmin => HasRole(Roles.Admin);

        private readonly HttpContext _context;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor.HttpContext;
        }

        public bool HasRole(string role)
        {
            if (!Roles.IsValidRole(role))
                return false;

            ClaimsIdentity? identity = _context.User.Identity as ClaimsIdentity;
            Claim? result = identity?.FindAll(ClaimTypes.Role)
                                     .Where(x => x.Value == role)
                                     .FirstOrDefault();

            return result != null;
        }

        public string[] GetRoles()
        {
            ClaimsIdentity? identity = _context.User.Identity as ClaimsIdentity;
            string[]? roles = identity?.FindAll(ClaimTypes.Role)
                                       .Select(x => x.Value)
                                       .ToArray();

            return roles ?? Array.Empty<string>();
        }

        public static Guid? GetUserIdFromContext(IHttpContextAccessor context)
        {
            return GetUserIdFromContext(context.HttpContext);
        }

        public static Guid? GetUserIdFromContext(HttpContext context)
        {
            ClaimsIdentity? identity = context.User.Identity as ClaimsIdentity;
            Claim? claim = identity?.FindFirst(ClaimTypes.UserData);

            return claim == null ? null : (Guid?)Guid.Parse(claim.Value);
        }
    }
}
