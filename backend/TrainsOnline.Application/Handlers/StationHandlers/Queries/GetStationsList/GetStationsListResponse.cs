namespace TrainsOnline.Application.Handlers.StationHandlers.Queries.GetStationsList
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.Mapping;
    using TrainsOnline.Domain.Entities;

    public class GetStationsListResponse : IDataTransferObject
    {
        public List<StationLookupModel> Stations { get; set; } = default!;

        public class StationLookupModel : IDataTransferObject, ICustomMapping
        {
            public Guid Id { get; set; }

            public string Name { get; set; } = default!;

            void ICustomMapping.CreateMappings(Profile configuration)
            {
                configuration.CreateMap<Station, StationLookupModel>();
            }
        }
    }
}
