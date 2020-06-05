namespace TrainsOnline.Tracker.Application.DTO.Station
{
    using System;
    using System.Collections.Generic;

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
