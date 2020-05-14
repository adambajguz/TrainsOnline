namespace TrainsOnline.Desktop.Domain.ValueObjects.UserComponents
{
    using System.Collections.Generic;
    using TrainsOnline.Desktop.Domain.ValueObjects.Base;

    public class Credentials : ValueObject
    {
        public string Email { get; set; }
        public bool IsAdmin { get; set; }

        public Credentials(string email, bool isAdmin)
        {
            Email = email;
            IsAdmin = isAdmin;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Email;
            yield return IsAdmin;
        }
    }
}
