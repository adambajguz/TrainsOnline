namespace TrainsOnline.Application.Handlers.UserHandlers.Commands.CreateUser
{
    using Application.Interfaces.Mapping;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Domain.Entities;

    public class CreateUserRequest : IDataTransferObject, ICustomMapping
    {
        public string? Email { get; set; }
        public string? Password { get; set; }

        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        public bool IsAdmin { get; set; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<CreateUserRequest, User>();
        }
    }
}
