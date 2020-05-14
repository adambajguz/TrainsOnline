namespace TrainsOnline.Desktop.Domain.DTO.User
{
    using System;
    using System.Collections.Generic;
    using TrainsOnline.Desktop.Domain.DTO;

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
