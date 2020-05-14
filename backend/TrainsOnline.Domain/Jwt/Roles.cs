namespace TrainsOnline.Domain.Jwt
{
    using System;
    using System.Linq;

    public sealed class Roles
    {
        public const string User = "User";
        public const string ResetPassword = "ResetPassword";
        public const string Admin = "Admin";

        public static string[] All { get; } = new string[]
        {
            User,
            ResetPassword,
            Admin
        };

        public static bool IsValidRole(string role)
        {
            return All.Contains(role);
        }

        public static string Build(params string[] roles)
        {
            return string.Join(',', roles);
        }

        public static string[] BuildArray(params string[] roles)
        {
            return roles;
        }
    }
}
