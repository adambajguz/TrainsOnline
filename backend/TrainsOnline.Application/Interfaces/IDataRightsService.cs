namespace TrainsOnline.Application.Interfaces
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IDataRightsService
    {
        public Guid? UserId { get; }
        public bool IsAuthenticated { get; }
        public bool IsAdmin { get; }

        bool HasRole(string role);
        string[] GetRoles();

        Task ValidateUserId(Guid userIdToValidate);
        Task ValidateUserId<T>(T? model, Expression<Func<T, Guid>> userIdFieldExpression) where T : class;
        void ValidateHasRole(string role);
        void ValidateIsAdmin();
    }
}
