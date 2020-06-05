namespace TrainsOnline.Tracker.Application.DTO.User
{
    using System;
    using TrainsOnline.Tracker.Application.DTO;

    public class GetUserDetailsResponse : IDataTransferObject
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public bool IsAdmin { get; set; }
    }
}
