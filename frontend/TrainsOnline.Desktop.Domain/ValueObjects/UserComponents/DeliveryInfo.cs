namespace TrainsOnline.Desktop.Domain.ValueObjects.UserComponents
{
    using System.Collections.Generic;
    using TrainsOnline.Desktop.Domain.ValueObjects.Base;

    public class DeliveryInfo : ValueObject
    {
        public string Name { get; }
        public string Surname { get; }
        public string PhoneNumber { get; }
        public string Address { get; }

        public DeliveryInfo(string name, string surname, string phoneNumber, string address)
        {
            Name = name;
            Surname = surname;
            PhoneNumber = phoneNumber;
            Address = address;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Surname;
            yield return PhoneNumber;
            yield return Address;
        }
    }
}
