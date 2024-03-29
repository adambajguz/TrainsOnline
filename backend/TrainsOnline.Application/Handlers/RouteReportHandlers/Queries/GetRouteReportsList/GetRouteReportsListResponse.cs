﻿namespace TrainsOnline.Application.Handlers.RouteReportHandlers.Queries.GetRouteReportsList
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.Mapping;
    using TrainsOnline.Domain.Entities;

    public class GetRouteReportsListResponse : IDataTransferObject
    {
        public IList<RouteReportLookupModel> RouteReports { get; set; } = default!;

        public class RouteReportLookupModel : IDataTransferObject, ICustomMapping
        {
            public Guid Id { get; set; }

            public DateTime CreatedOn { get; set; }
            public DateTime LastSavedOn { get; set; }

            //public Guid RouteId { get; set; }

            //public string FromName { get; set; } = default!;
            //public string ToName { get; set; } = default!;

            //public double VoltageMean { get; set; }
            //public double CurrentMean { get; set; }
            //public double PowerMean { get; set; }
            //public double SpeedMean { get; set; }
            //public TimeSpan Duration { get; set; }
            //public int NumberOfStops { get; set; }
            //public double StopDurationMean { get; set; }
            //public double StopDurationMax { get; set; }

            public Guid RouteId { get; set; }

            public string FromName { get; set; } = default!;
            public string ToName { get; set; } = default!;

            public double VoltageMin { get; set; }
            public double VoltageMean { get; set; }
            public double VoltageSd { get; set; }
            public double VoltageMax { get; set; }

            public double CurrentMean { get; set; }
            public double CurrentSd { get; set; }
            public double CurrentMax { get; set; }

            public double PowerMean { get; set; }
            public double PowerSd { get; set; }
            public double PowerMax { get; set; }

            public double SpeedMean { get; set; }
            public double SpeedSd { get; set; }
            public double SpeedMax { get; set; }

            public TimeSpan Duration { get; set; }
            public int NumberOfStops { get; set; }

            public double StopDurationMean { get; set; }
            public double StopDurationSd { get; set; }
            public double StopDurationMax { get; set; }

            void ICustomMapping.CreateMappings(Profile configuration)
            {
                configuration.CreateMap<RouteReport, RouteReportLookupModel>();
            }
        }
    }
}
