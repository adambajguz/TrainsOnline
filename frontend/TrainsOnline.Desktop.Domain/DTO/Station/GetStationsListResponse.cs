namespace TrainsOnline.Desktop.Domain.DTO.Station
{
    using System;
    using System.Collections.Generic;
    using TrainsOnline.Desktop.Domain.DTO;

    public class GetStationsListResponse : IDataTransferObject
    {
        public List<StationLookupModel> Stations { get; set; }

        public class StationLookupModel : IDataTransferObject
        {
            public Guid Id { get; set; }

            public string Name { get; set; }
        }
    }
}
