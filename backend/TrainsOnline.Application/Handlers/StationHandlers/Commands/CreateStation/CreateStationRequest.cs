namespace TrainsOnline.Application.Handlers.StationHandlers.Commands.CreateStation
{
    using Application.Interfaces.Mapping;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Domain.Entities;

    public class CreateStationRequest : IDataTransferObject, ICustomMapping
    {
        public string? Name { get; set; } = default!;

        public double Latitude { get; set; } = default!;
        public double Longitude { get; set; } = default!;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<CreateStationRequest, Station>();
        }
    }
}
