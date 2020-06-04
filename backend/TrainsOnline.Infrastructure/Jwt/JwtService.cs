namespace TrainsOnline.Infrastructure.Jwt
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using TrainsOnline.Application.Handlers.AuthenticationHandlers.Queries.GetValidToken;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Domain.Jwt;

    public class JwtService : IJwtService
    {
        private readonly JwtSettings _settings;
        private readonly byte[] _key;
        private readonly JwtSecurityTokenHandler _handler;

        public JwtService(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
            _key = Base64UrlEncoder.DecodeBytes(_settings.Key);
            _handler = new JwtSecurityTokenHandler();
        }

        public JwtTokenModel GenerateJwtToken(string email, Guid id, string[] roles)
        {
            ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.UserData, id.ToString(), id.GetType().Name)
                });

            if (roles.Length == 0)
                throw new InvalidOperationException("Roles contains no elements");

            for (int i = 0; i < roles.Length; ++i)
                if (!Roles.IsValidRole(roles[i]))
                    throw new InvalidOperationException("Invalid role");
                else
                    claims.AddClaim(new Claim(ClaimTypes.Role, roles[i]));

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.Add(_settings.Lease),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha512Signature)
            };
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            SecurityToken result = _handler.CreateToken(tokenDescriptor);

            return new JwtTokenModel
            {
                Token = _handler.WriteToken(result),
                Lease = _settings.Lease,
                ValidTo = result.ValidTo
            };
        }

        public void ValidateStringToken(string? token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Token is null or empty", nameof(token));

            _handler.ValidateToken(token, GetValidationParameters(_key), out _);
        }

        public static TokenValidationParameters GetValidationParameters(byte[] key)
        {
            //Issue - who 
            //Audience - to whom
            return new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
                RequireSignedTokens = true
            };
        }

        public bool IsTokenStringValid(string? token)
        {
            if (token is null)
                return false;

            try
            {
                ValidateStringToken(token);
                return true;
            }
            catch (Exception)
            {

            }

            return false;
        }

        public Guid GetUserIdFromToken(string token)
        {
            JwtSecurityToken secToken = _handler.ReadJwtToken(token);
            Claim? claim = secToken.Claims.FirstOrDefault(x => x.Type.Equals("userdata") || x.Type.Equals(ClaimTypes.UserData));
            Guid userId = Guid.Parse(claim?.Value!);

            return userId;
        }

        public bool IsRoleInToken(string? token, string role)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            if (!Roles.IsValidRole(role))
                return false;

            JwtSecurityToken jwtToken = _handler.ReadJwtToken(token);
            List<Claim> claims = jwtToken.Claims.Where(x => x.Type.Equals("role") || x.Type.Equals(ClaimTypes.Role)).ToList();

            return claims.FirstOrDefault(x => x.Value.Equals(role)) != null;
        }

        public bool IsAnyOfRolesInToken(string? token, string[] roles)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            //if (!Roles.IsValidRole(role))
            //    return false;

            JwtSecurityToken jwtToken = _handler.ReadJwtToken(token);
            List<Claim> claims = jwtToken.Claims.Where(x => x.Type.Equals("role") || x.Type.Equals(ClaimTypes.Role)).ToList();

            IEnumerable<string> intersection = claims.Select(x => x.Value).Intersect(roles);

            return intersection.Any();
        }
    }
}
