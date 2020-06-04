namespace TrainsOnline.Desktop.Infrastructure.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;

    internal class JwtTokenHelper
    {
        private readonly JwtSecurityTokenHandler _handler = new JwtSecurityTokenHandler();

        public JwtTokenHelper()
        {

        }

        public Guid GetUserIdFromToken(string token)
        {
            JwtSecurityToken secToken = _handler.ReadJwtToken(token);
            Claim claim = secToken.Claims.FirstOrDefault(x => x.Type.Equals("userdata") || x.Type.Equals(ClaimTypes.UserData));
            Guid userId = Guid.Parse(claim?.Value);

            return userId;
        }

        public bool IsRoleInToken(string token, string role)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            JwtSecurityToken jwtToken = _handler.ReadJwtToken(token);
            List<Claim> claims = jwtToken.Claims.Where(x => x.Type.Equals("role") || x.Type.Equals(ClaimTypes.Role)).ToList();

            return claims.FirstOrDefault(x => x.Value.Equals(role)) != null;
        }

        public bool IsAnyOfRolesInToken(string token, string[] roles)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            JwtSecurityToken jwtToken = _handler.ReadJwtToken(token);
            List<Claim> claims = jwtToken.Claims.Where(x => x.Type.Equals("role") || x.Type.Equals(ClaimTypes.Role)).ToList();

            IEnumerable<string> intersection = claims.Select(x => x.Value).Intersect(roles);

            return intersection.Any();
        }
    }
}
