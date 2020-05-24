﻿namespace TrainsOnline.Infrastructure.Repository
{
    using Application.Interfaces;
    using AutoMapper;
    using TrainsOnline.Application.Interfaces.DbContext;
    using TrainsOnline.Application.Interfaces.Repository;
    using TrainsOnline.Domain.Entities;

    public class RoutesRepository : GenericRelationalRepository<Route>, IRoutesRepository
    {
        public RoutesRepository(ICurrentUserService currentUserService,
                                ITrainsOnlineRelationalDbContext context,
                                IMapper mapper) : base(currentUserService, context, mapper)
        {

        }
    }
}
