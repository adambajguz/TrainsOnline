namespace TrainsOnline.Application.Interfaces
{
    using System;

    public interface ICurrentUserService
    {
        public Guid? UserId { get; }
        public bool IsAuthenticated { get; }
        public bool IsAdmin { get; }

        bool HasRole(string role);
        string[] GetRoles();
    }
}