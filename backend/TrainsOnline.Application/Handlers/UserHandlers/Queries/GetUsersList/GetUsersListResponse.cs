namespace TrainsOnline.Application.Handlers.UserHandlers.Queries.GetUsersList
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Mapping;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Domain.Entities;

    public class GetUsersListResponse : IDataTransferObject
    {
        public IList<UserLookupModel> Users { get; set; } = default!;

        public class UserLookupModel : IDataTransferObject, ICustomMapping
        {
            public Guid Id { get; set; }

            public string Email { get; set; } = default!;

            void ICustomMapping.CreateMappings(Profile configuration)
            {
                configuration.CreateMap<User, UserLookupModel>();
            }
        }
    }
}
