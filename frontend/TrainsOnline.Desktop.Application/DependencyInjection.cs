namespace TrainsOnline.Desktop.Core
{
    using System.Reflection;
    using AutoMapper;
    using Caliburn.Micro;
    using TrainsOnline.Desktop.Application.Mappings;

    public static class DependencyInjection
    {
        public static void AddApplication(this SimpleContainer _container)
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile(typeof(TrainsOnline.Desktop.Core.DependencyInjection).GetTypeInfo().Assembly));
            });

            _container.RegisterInstance(
                typeof(IMapper),
                typeof(IMapper).FullName,
                config.CreateMapper()
            );
        }
    }
}
