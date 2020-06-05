namespace TrainsOnline.Tracker.Application.DTO.User
{
    using System;
    using System.Collections.Generic;
    using TrainsOnline.Tracker.Application.DTO;

    public class GetUsersListResponse : IDataTransferObject
    {
        public List<UserLookupModel> Users { get; set; }

        public class UserLookupModel : IDataTransferObject
        {
            public Guid Id { get; set; }

            public string Email { get; set; }
        }
    }
}
