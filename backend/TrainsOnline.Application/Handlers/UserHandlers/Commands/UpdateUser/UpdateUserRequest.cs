namespace TrainsOnline.Application.Handlers.UserHandlers.Commands.UpdateUser
{
    using System;
    using Application.Interfaces.Mapping;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Domain.Entities;

    public class UpdateUserRequest : IDataTransferObject, ICustomMapping
    {
        public Guid Id { get; set; }

        public string? Email { get; set; }

        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        public bool IsAdmin { get; set; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<UpdateUserRequest, User>();
        }
    }
}
