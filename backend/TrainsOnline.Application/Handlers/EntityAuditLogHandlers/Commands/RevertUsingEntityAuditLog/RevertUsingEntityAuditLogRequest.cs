﻿namespace TrainsOnline.Application.Handlers.EntityAuditLog.Commands.CreateRouteLog
{
    using System;
    using Application.Interfaces.Mapping;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Domain.Entities;

    public class RevertUsingEntityAuditLogRequest : IDataTransferObject, ICustomMapping
    {
        public Guid FromId { get; set; }
        public Guid ToId { get; set; }

        public DateTime DepartureTime { get; set; } = default!;
        public TimeSpan Duration { get; set; } = default!;
        public double Distance { get; set; }
        public double TicketPrice { get; set; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<RevertUsingEntityAuditLogRequest, RouteLog>();
        }
    }
}