namespace TrainsOnline.Desktop.Domain.DTO.User
{
    using TrainsOnline.Desktop.Domain.DTO;

    public class CreateUserRequest : IDataTransferObject
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public bool IsAdmin { get; set; }
    }
}
