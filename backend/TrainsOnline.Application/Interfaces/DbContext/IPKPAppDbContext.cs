﻿namespace TrainsOnline.Application.Interfaces.DbContext
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public interface IPKPAppDbContext : IGenericDatabaseContext
    {
        DbSet<EntityAuditLog> EntityAuditLogs { get; set; }

        DbSet<Route> Routes { get; set; }
        DbSet<Station> Stations { get; set; }
        DbSet<Ticket> Tickets { get; set; }
        DbSet<User> Users { get; set; }
    }
}
