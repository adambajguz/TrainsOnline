namespace TrainsOnline.Application.Handlers.StationHandlers.Commands.UpdateStation
{
    using System;
    using Application.Interfaces.Mapping;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Domain.Entities;

    public class UpdateStationRequest : IDataTransferObject, ICustomMapping
    {
        public Guid Id { get; set; }

        public string? Name { get; set; } = default!;

        public double Latitude { get; set; } = default!;
        public double Longitude { get; set; } = default!;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<UpdateStationRequest, Station>();
        }
    }
}
